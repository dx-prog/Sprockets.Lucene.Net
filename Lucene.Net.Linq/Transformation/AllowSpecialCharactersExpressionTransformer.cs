/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;

namespace Sprockets.Lucene.Net.Linq.Transformation {
    internal class AllowSpecialCharactersExpressionTransformer : IExpressionTransformer<MethodCallExpression> {
        public Expression Transform(MethodCallExpression expression) {
            if (expression.Method.Name != ReflectionUtility
                    .GetMethod(() => LuceneMethods.AllowSpecialCharacters<object>(null))
                    .Name)
                return expression;

            return new AllowSpecialCharactersExpression(expression.Arguments[0]);
        }

        public ExpressionType[] SupportedExpressionTypes => new[] {ExpressionType.Call};
    }
}
