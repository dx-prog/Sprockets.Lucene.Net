/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using Remotion.Linq.Parsing;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Removes method calls like string.ToLower() that have no effect on a query due to
    ///     case sensitivity in Lucene being configured elsewhere by the Analyzer.
    /// </summary>
    internal class NoOpMethodCallRemovingTreeVisitor : RelinqExpressionVisitor {
        private static readonly ISet<string> NoOpMethods =
            new HashSet<string> {
                "ToLower",
                "ToLowerInvariant",
                "ToUpper",
                "ToUpeprInvariant"
            };

        protected override Expression VisitMethodCall(MethodCallExpression expression) {
            if (NoOpMethods.Contains(expression.Method.Name))
                return expression.Object;

            return base.VisitMethodCall(expression);
        }
    }
}
