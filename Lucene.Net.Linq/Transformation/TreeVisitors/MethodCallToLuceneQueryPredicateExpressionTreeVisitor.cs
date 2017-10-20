/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Lucene.Net.Search;
using Remotion.Linq.Parsing;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces supported method calls like [LuceneQueryFieldExpression].StartsWith("foo") with a
    ///     LuceneQueryPredicateExpression like [LuceneQueryPredicateExpression](+Field:foo*)
    /// </summary>
    internal class MethodCallToLuceneQueryPredicateExpressionTreeVisitor : RelinqExpressionVisitor {
        protected override Expression VisitMethodCall(MethodCallExpression expression) {
            var queryField = expression.Object as LuceneQueryFieldExpression;

            if (queryField == null)
                return base.VisitMethodCall(expression);

            if (expression.Method.Name == "StartsWith")
                return new LuceneQueryPredicateExpression(queryField,
                    expression.Arguments[0],
                    Occur.MUST,
                    QueryType.Prefix);
            if (expression.Method.Name == "EndsWith")
                return new LuceneQueryPredicateExpression(queryField,
                    expression.Arguments[0],
                    Occur.MUST,
                    QueryType.Suffix);
            if (expression.Method.Name == "Contains")
                return new LuceneQueryPredicateExpression(queryField,
                    expression.Arguments[0],
                    Occur.MUST,
                    QueryType.Wildcard);

            return base.VisitMethodCall(expression);
        }
    }
}
