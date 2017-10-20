/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneQueryAnyFieldExpression : LuceneQueryFieldExpression {
        private static readonly LuceneQueryAnyFieldExpression instance = new LuceneQueryAnyFieldExpression();

        private LuceneQueryAnyFieldExpression()
            : base(typeof(string), (ExpressionType) LuceneExpressionType.LuceneQueryAnyFieldExpression, "*") {
        }

        public static Expression Instance => instance;

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            return this;
        }
    }
}
