/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using Remotion.Linq.Clauses.ResultOperators;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal class SkipResultOperatorHandler : ResultOperatorHandler<SkipResultOperator> {
        protected override void AcceptInternal(SkipResultOperator skip, LuceneQueryModel model) {
            var additionalSkip = skip.GetConstantCount();
            model.SkipResults += additionalSkip;

            if (model.MaxResults != int.MaxValue)
                model.MaxResults -= additionalSkip;
        }
    }
}
