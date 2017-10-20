/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Abstractions {
    /// <summary>
    ///     Abstraction of IndexWriter to faciliate unit testing.
    /// </summary>
    /// <see cref="IndexWriter" />
    public interface IIndexWriter : IDisposable {
        /// <summary>
        ///     Gets a value indicating whether this instance has been closed either
        ///     by <see cref="Dispose" /> or <see cref="Rollback" /> being called.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        bool IsClosed { get; }

        /// <see cref="IndexWriter.AddDocument(Lucene.Net.Documents.Document)" />
        void AddDocument(Document doc);

        /// <see cref="IndexWriter.DeleteDocuments(Lucene.Net.Search.Query[])" />
        void DeleteDocuments(Query[] queries);

        /// <see cref="IndexWriter.DeleteAll" />
        void DeleteAll();

        /// <see cref="IndexWriter.Commit()" />
        void Commit();

        /// <see cref="IndexWriter.Rollback" />
        void Rollback();

        /// <see cref="IndexWriter.Optimize()" />
        void Optimize();

        /// <see cref="IndexWriter.GetReader()" />
        IndexReader GetReader();
    }
}
