/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Remotion.Linq;
using Sprockets.Lucene.Net.Linq.Clauses;

namespace Sprockets.Lucene.Net.Linq {
    internal interface ILuceneQueryModelVisitor : IQueryModelVisitor {
        void VisitBoostClause(BoostClause clause, QueryModel queryModel, int index);
        void VisitTrackRetrievedDocumentsClause(TrackRetrievedDocumentsClause clause, QueryModel queryModel, int index);
        void VisitQueryStatisticsCallbackClause(QueryStatisticsCallbackClause clause, QueryModel queryModel, int index);
    }
}
