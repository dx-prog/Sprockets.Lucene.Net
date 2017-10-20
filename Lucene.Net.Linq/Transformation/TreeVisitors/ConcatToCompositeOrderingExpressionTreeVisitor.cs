/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq.Parsing;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces method calls like string.Concat([LuceneQueryFieldExpression], [LuceneQueryFieldExpression]) to
    ///     LuceneCompositeOrderingExpression
    /// </summary>
    internal class ConcatToCompositeOrderingExpressionTreeVisitor : RelinqExpressionVisitor {
        protected override Expression VisitMethodCall(MethodCallExpression expression) {
            if (expression.Method.Name == "Concat" && expression.Arguments.Count == 2) {
                var fields = new[] {
                    (LuceneQueryFieldExpression) expression.Arguments[0],
                    (LuceneQueryFieldExpression) expression.Arguments[1]
                };
                return new LuceneCompositeOrderingExpression(fields);
            }

            return base.VisitMethodCall(expression);
        }
    }
}
