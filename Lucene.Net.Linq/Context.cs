/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace Sprockets.Lucene.Net.Linq {
    internal class Context : IDisposable {
        private static readonly ILog Log = LogManager.GetLogger<Context>();

        private readonly object reloadLock = new object();

        private readonly object searcherLock = new object();
        private bool disposed;
        private IndexReader reader;
        private SearcherClientTracker tracker;

        public Context(Directory directory, object transactionLock) {
            Directory = directory;
            TransactionLock = transactionLock;
            Settings = new LuceneDataProviderSettings();
        }

        public LuceneDataProviderSettings Settings { get; set; }

        public Directory Directory { get; }

        public object TransactionLock { get; }

        internal SearcherClientTracker CurrentTracker {
            get {
                lock (searcherLock) {
                    AssertNotDisposed();

                    if (tracker == null) {
                        var searcher = CreateSearcher();
                        reader = searcher.IndexReader;
                        tracker = new SearcherClientTracker(searcher);
                    }
                    return tracker;
                }
            }
        }

        public void Dispose() {
            lock (searcherLock) {
                AssertNotDisposed();

                disposed = true;

                if (tracker == null)
                    return;

                if (!tracker.TryDispose())
                    Log.Warn(m => m("Context is being disposed before all handles were released."));

                tracker = null;
            }
        }

        public event EventHandler<SearcherLoadEventArgs> SearcherLoading;

        public ISearcherHandle CheckoutSearcher() {
            AssertNotDisposed();
            return new SearcherHandle(CurrentTracker);
        }

        public virtual void Reload() {
            lock (reloadLock) {
                AssertNotDisposed();
                Log.Info(m => m("Reloading index."));

                IndexSearcher searcher;
                if (reader == null) {
                    searcher = CreateSearcher();
                    reader = searcher.IndexReader;
                }
                else if (!ReopenSearcher(out searcher)) {
                    return;
                }

                var newTracker = new SearcherClientTracker(searcher);

                var tmpHandler = SearcherLoading;

                if (tmpHandler != null) {
                    Log.Debug(m => m("Invoking SearcherLoading event."));
                    tmpHandler(this, new SearcherLoadEventArgs(newTracker.Searcher));
                }

                lock (searcherLock) {
                    if (tracker != null)
                        tracker.Dispose();

                    tracker = newTracker;
                }
            }

            Log.Debug(m => m("Index reloading completed."));
        }

        protected virtual IndexSearcher CreateSearcher() {
            return new IndexSearcher(IndexReader.Open(Directory, true));
        }

        /// <summary>
        ///     Reopen the <see cref="IndexReader" />. If the index has not changed,
        ///     return <c>false</c>. If the index has changed, set <paramref name="searcher" />
        ///     with a new <see cref="IndexSearcher" /> instance and return <c>true</c>.
        /// </summary>
        protected virtual bool ReopenSearcher(out IndexSearcher searcher) {
            searcher = null;
            var oldReader = reader;
            reader = reader.Reopen();
            if (ReferenceEquals(reader, oldReader))
                return false;

            searcher = new IndexSearcher(reader);
            return true;
        }

        private void AssertNotDisposed() {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        internal class SearcherHandle : ISearcherHandle {
            private readonly SearcherClientTracker tracker;
            private bool disposed;

            public SearcherHandle(SearcherClientTracker tracker) {
                this.tracker = tracker;
                tracker.AddClient(this);
            }

            public IndexSearcher Searcher => tracker.Searcher;

            public void Dispose() {
                if (disposed)
                    throw new ObjectDisposedException(typeof(ISearcherHandle).Name);

                disposed = true;
                tracker.RemoveClient(this);
            }
        }

        internal class SearcherClientTracker : IDisposable {
            private static readonly IList<SearcherClientTracker> undisposedTrackers = new List<SearcherClientTracker>();
            private readonly List<WeakReference> searcherReferences = new List<WeakReference>();

            private readonly object sync = new object();
            private bool disposed;
            private bool disposePending;

            public SearcherClientTracker(IndexSearcher searcher) {
                Searcher = searcher;

                lock (typeof(SearcherClientTracker)) {
                    undisposedTrackers.Add(this);
                }
            }

            public IndexSearcher Searcher { get; }

            internal int ReferenceCount {
                get {
                    lock (sync) {
                        return searcherReferences.Count;
                    }
                }
            }

            public void Dispose() {
                TryDispose();
            }

            public void AddClient(object client) {
                lock (sync) {
                    searcherReferences.Add(new WeakReference(client));
                }
            }

            public void RemoveClient(object client) {
                lock (sync) {
                    searcherReferences.Remove(searcherReferences.First(wr => ReferenceEquals(wr.Target, client)));
                    RemoveDeadReferences();

                    if (disposePending)
                        Dispose();
                }
            }

            public bool TryDispose() {
                lock (sync) {
                    disposePending = false;

                    if (disposed)
                        throw new ObjectDisposedException(GetType().Name);

                    RemoveDeadReferences();
                    if (searcherReferences.Count == 0) {
                        lock (typeof(SearcherClientTracker)) {
                            undisposedTrackers.Remove(this);
                        }

                        var reader = Searcher.IndexReader;
                        Searcher.Dispose();
                        // NB IndexSearcher.Dispose() does not Dispose externally provided IndexReader:
                        reader.Dispose();

                        disposed = true;
                    }
                    else {
                        disposePending = true;
                    }

                    return disposed;
                }
            }

            private void RemoveDeadReferences() {
                searcherReferences.RemoveAll(wr => !wr.IsAlive);
            }
        }
    }
}
