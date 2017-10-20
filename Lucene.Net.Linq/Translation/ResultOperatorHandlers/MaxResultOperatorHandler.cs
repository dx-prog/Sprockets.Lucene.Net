/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal class MaxResultOperatorHandler : ResultOperatorHandler<MaxResultOperator> {
        protected override void AcceptInternal(MaxResultOperator resultOperator, LuceneQueryModel model) {
            model.ResetSorts();
            model.AddSort(model.SelectClause, OrderingDirection.Desc);
            model.MaxResults = 1;
        }
    }
}
