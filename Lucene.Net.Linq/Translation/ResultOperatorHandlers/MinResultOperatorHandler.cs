/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal class MinResultOperatorHandler : ResultOperatorHandler<MinResultOperator> {
        protected override void AcceptInternal(MinResultOperator resultOperator, LuceneQueryModel model) {
            model.ResetSorts();
            model.AddSort(model.SelectClause, OrderingDirection.Asc);
            model.MaxResults = 1;
            model.Aggregate = true;
        }
    }
}
