/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Clauses.TreeVisitors;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    internal class FlagToBinaryConditionTreeVisitor : LuceneExpressionTreeVisitor {
        private bool negate;
        private Expression parent;

        protected override Expression VisitBinary(BinaryExpression expression) {
            var oldParent = parent;

            parent = expression;

            try {
                return base.VisitBinary(expression);
            }
            finally {
                parent = oldParent;
            }
        }

        protected override Expression VisitUnary(UnaryExpression expression) {
            if (expression.NodeType != ExpressionType.Not || expression.Type != typeof(bool))
                return base.VisitUnary(expression);

            negate = !negate;

            var operand = Visit(expression.Operand);

            negate = !negate;

            if (Equals(operand, expression.Operand))
                return Expression.MakeBinary(ExpressionType.Equal, operand, Expression.Constant(negate));

            return operand;
        }

        protected override Expression VisitLuceneQueryFieldExpression(LuceneQueryFieldExpression expression) {
            if (expression.Type != typeof(bool) || IsAlreadyInEqualityExpression())
                return base.VisitLuceneQueryFieldExpression(expression);

            return Expression.MakeBinary(ExpressionType.Equal, expression, Expression.Constant(!negate));
        }

        private bool IsAlreadyInEqualityExpression() {
            if (!(parent is BinaryExpression))
                return false;

            var binary = (BinaryExpression) parent;

            return binary.NodeType == ExpressionType.Equal || binary.NodeType == ExpressionType.NotEqual;
        }
    }
}
