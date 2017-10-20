/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using System.Reflection;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces method calls like <c cref="LuceneMethods.Matches{T}">Matches</c> with query expressions.
    /// </summary>
    internal class ExternallyProvidedQueryExpressionTreeVisitor : MethodInfoMatchingTreeVisitor {
        private static readonly MethodInfo MatchesMethod =
            ReflectionUtility.GetMethod(() => LuceneMethods.Matches<object>(null, null));

        internal ExternallyProvidedQueryExpressionTreeVisitor() {
            AddMethod(MatchesMethod);
        }

        protected override Expression VisitSupportedMethodCallExpression(MethodCallExpression expression) {
            return new LuceneQueryExpression((Query) ((ConstantExpression) expression.Arguments[0]).Value);
        }
    }
}
