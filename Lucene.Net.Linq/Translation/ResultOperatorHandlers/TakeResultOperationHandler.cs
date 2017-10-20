/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using Remotion.Linq.Clauses.ResultOperators;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal class TakeResultOperationHandler : ResultOperatorHandler<TakeResultOperator> {
        protected override void AcceptInternal(TakeResultOperator take, LuceneQueryModel model) {
            model.MaxResults = Math.Min(take.GetConstantCount(), model.MaxResults);
        }
    }
}
