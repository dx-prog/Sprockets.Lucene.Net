/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq {
    internal class QueryExecutionContext : IQueryExecutionContext {
        public QueryExecutionContext() {
        }

        public QueryExecutionContext(IndexSearcher searcher, Query query, Filter filter) {
            Searcher = searcher;
            Query = query;
            Filter = filter;
        }

        public QueryExecutionPhase Phase { get; set; }
        public IndexSearcher Searcher { get; set; }
        public Query Query { get; set; }
        public Filter Filter { get; set; }
        public TopFieldDocs Hits { get; set; }
        public int CurrentHit { get; set; }
        public ScoreDoc CurrentScoreDoc { get; set; }
    }
}
