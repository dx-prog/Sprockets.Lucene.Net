/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces subqueries like {[doc].Tags => Contains("c")} with BinaryExpressions like ([doc].Tags == "c").
    /// </summary>
    internal class SubQueryContainsTreeVisitor : RelinqExpressionVisitor {
        protected override Expression VisitSubQuery(SubQueryExpression expression) {
            var operators = expression.QueryModel.ResultOperators;

            if (operators.Count == 1 && operators[0] is ContainsResultOperator) {
                var op = (ContainsResultOperator) operators[0];

                var field = expression.QueryModel.MainFromClause.FromExpression;
                var pattern = op.Item;
                if (pattern.Type.IsPrimitive)
                    pattern = Expression.Constant(((ConstantExpression) pattern).Value, typeof(object));

                return Expression.MakeBinary(ExpressionType.Equal, field, pattern);
            }

            return base.VisitSubQuery(expression);
        }
    }
}
