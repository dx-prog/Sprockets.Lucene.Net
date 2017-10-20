/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;

namespace Sprockets.Lucene.Net.Linq.Clauses {
    internal class BoostClause : ExtensionClause<LambdaExpression> {
        public BoostClause(LambdaExpression expression) : base(expression) {
        }

        public LambdaExpression BoostFunction => expression;

        public override IBodyClause Clone(CloneContext cloneContext) {
            return new BoostClause(expression);
        }

        public override string ToString() {
            return "boost " + expression;
        }

        protected override void Accept(ILuceneQueryModelVisitor visitor, QueryModel queryModel, int index) {
            visitor.VisitBoostClause(this, queryModel, index);
        }
    }
}
