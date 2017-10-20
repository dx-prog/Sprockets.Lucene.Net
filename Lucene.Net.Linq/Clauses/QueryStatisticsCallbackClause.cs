/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;

namespace Sprockets.Lucene.Net.Linq.Clauses {
    internal class QueryStatisticsCallbackClause : ExtensionClause<ConstantExpression> {
        public QueryStatisticsCallbackClause(ConstantExpression expression)
            : base(expression) {
        }

        public Action<LuceneQueryStatistics> Callback => (Action<LuceneQueryStatistics>) expression.Value;

        public override IBodyClause Clone(CloneContext cloneContext) {
            return new QueryStatisticsCallbackClause(expression);
        }

        protected override void Accept(ILuceneQueryModelVisitor visitor, QueryModel queryModel, int index) {
            visitor.VisitQueryStatisticsCallbackClause(this, queryModel, index);
        }
    }
}
