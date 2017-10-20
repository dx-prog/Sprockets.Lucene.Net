/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using Remotion.Linq.Clauses.ResultOperators;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal class LastResultOperatorHandler : ResultOperatorHandler<LastResultOperator> {
        protected override void AcceptInternal(LastResultOperator resultOperator, LuceneQueryModel model) {
            model.Last = true;
        }
    }
}
