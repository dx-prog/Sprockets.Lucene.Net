/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq {
    /// <summary>
    ///     Provides access to statistics about queries via <see cref="LuceneMethods.CaptureStatistics{T}" />.
    /// </summary>
    public class LuceneQueryStatistics {
        public LuceneQueryStatistics(Query query,
            Filter filter,
            Sort sort,
            TimeSpan elapsedPreparationTime,
            TimeSpan elapsedSearchTime,
            TimeSpan elapsedRetrievalTime,
            int totalHits,
            int skippedHits,
            int retrievedDocuments) {
            TotalHits = totalHits;
            Query = query;
            Filter = filter;
            Sort = sort;
            ElapsedPreparationTime = elapsedPreparationTime;
            ElapsedSearchTime = elapsedSearchTime;
            ElapsedRetrievalTime = elapsedRetrievalTime;
            SkippedHits = skippedHits;
            RetrievedDocuments = retrievedDocuments;
        }

        /// <summary>
        ///     The Query (generally a complex <see cref="BooleanQuery" /> or <see cref="MatchAllDocsQuery" />)
        ///     that was executed on
        ///     <see cref="Searcher.Search(Lucene.Net.Search.Query,Lucene.Net.Search.Filter,int,Lucene.Net.Search.Sort)" />
        /// </summary>
        public Query Query { get; }

        /// <summary>
        ///     The Filter (null when <see cref="LuceneDataProviderSettings.EnableMultipleEntities" /> is false)
        ///     that was executed on
        ///     <see cref="Searcher.Search(Lucene.Net.Search.Query,Lucene.Net.Search.Filter,int,Lucene.Net.Search.Sort)" />
        /// </summary>
        public Filter Filter { get; }

        /// <summary>
        ///     The Sort that was executed on
        ///     <see cref="Searcher.Search(Lucene.Net.Search.Query,Lucene.Net.Search.Filter,int,Lucene.Net.Search.Sort)" />
        /// </summary>
        public Sort Sort { get; }

        /// <summary>
        ///     Returns the total amount of time taken to translate the LINQ expression tree into a Lucene Query.
        /// </summary>
        public TimeSpan ElapsedPreparationTime { get; }

        /// <summary>
        ///     Returns the total amount of time spent in
        ///     <see cref="Searcher.Search(Lucene.Net.Search.Query,Lucene.Net.Search.Filter,int,Lucene.Net.Search.Sort)" />
        /// </summary>
        public TimeSpan ElapsedSearchTime { get; }

        /// <summary>
        ///     Returns the total amount of time spent converting <see cref="Document" /> and enumerating projected results.
        /// </summary>
        public TimeSpan ElapsedRetrievalTime { get; }

        /// <summary>
        ///     Returns the total hits that matched the query, including items that were not enumerated
        ///     due to <c>Skip</c> and <c>Take</c>.
        /// </summary>
        public int TotalHits { get; }

        /// <summary>
        ///     Returns the number of hits that were skipped by <see cref="Enumerable.Skip{TSource}" />
        /// </summary>
        public int SkippedHits { get; }

        /// <summary>
        ///     Returns the number of hits that were retrieved. This will generally be the lesser
        ///     of total hits or limit imposed by <see cref="Enumerable.Take{TSource}" />.
        /// </summary>
        public int RetrievedDocuments { get; }

        public override string ToString() {
            var sb = new StringBuilder("LuceneQueryStatistics { ");
            sb.Append("TotalHits: " + TotalHits);
            sb.Append(", Skipped: " + SkippedHits);
            sb.Append(", Retrieved: " + RetrievedDocuments);
            sb.Append(", ElapsedPreparationTime: " + ElapsedPreparationTime);
            sb.Append(", ElapsedSearchTime: " + ElapsedSearchTime);
            sb.Append(", ElapsedRetrievalTime: " + ElapsedRetrievalTime);
            sb.Append(", Query: " + Query);
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
