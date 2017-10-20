/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Abstractions {
    /// <summary>
    ///     Wraps an IndexWriter with an implementation of <c cref="IIndexWriter" />.
    /// </summary>
    public class IndexWriterAdapter : IIndexWriter {
        private readonly IndexWriter target;

        /// <param name="target">The IndexWriter instance to delegate method calls to.</param>
        public IndexWriterAdapter(IndexWriter target) {
            this.target = target;
        }

        public void DeleteAll() {
            target.DeleteAll();
        }

        public void DeleteDocuments(Query[] queries) {
            target.DeleteDocuments(queries);
        }

        public void Commit() {
            target.Commit();
        }

        public void AddDocument(Document doc) {
            target.AddDocument(doc);
        }

        public void Dispose() {
            IsClosed = true;
            target.Dispose();
        }

        public void Optimize() {
            target.Optimize();
        }

        public void Rollback() {
            IsClosed = true;
            target.Rollback();
        }

        public IndexReader GetReader() {
            return target.GetReader();
        }

        public bool IsClosed { get; private set; }
    }
}
