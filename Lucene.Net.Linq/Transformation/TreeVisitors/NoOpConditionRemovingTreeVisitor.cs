/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq.Parsing;
using Sprockets.Lucene.Net.Linq.Util;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Converts pointless BinaryExpressions like "True AndAlso Expression"
    ///     or "False OrElse Expression" to take only the right side.  Applies
    ///     recursively to collapse deeply nested pointless expressions.
    /// </summary>
    internal class NoOpConditionRemovingTreeVisitor : RelinqExpressionVisitor {
        protected override Expression VisitBinary(BinaryExpression expression) {
            var result = base.VisitBinary(expression);

            if (result is BinaryExpression)
                expression = (BinaryExpression) result;
            else
                return result;

            if (expression.NodeType == ExpressionType.AndAlso && expression.Left.IsTrueConstant())
                return expression.Right;

            if (expression.NodeType == ExpressionType.OrElse && expression.Left.IsFalseConstant())
                return expression.Right;

            return result;
        }
    }
}
