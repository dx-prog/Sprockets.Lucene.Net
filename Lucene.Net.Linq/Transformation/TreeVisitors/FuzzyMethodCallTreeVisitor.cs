/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq.Expressions;
using System.Reflection;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    internal class FuzzyMethodCallTreeVisitor : MethodInfoMatchingTreeVisitor {
        private static readonly MethodInfo FuzzyMethod = ReflectionUtility.GetMethod(() => false.Fuzzy(0f));

        internal FuzzyMethodCallTreeVisitor() {
            AddMethod(FuzzyMethod);
        }

        protected override Expression VisitSupportedMethodCallExpression(MethodCallExpression expression) {
            var query = expression.Arguments[0] as LuceneQueryPredicateExpression;

            if (query == null)
                throw new NotSupportedException(
                    "Fuzzy is only supported after predicate expressions. Example: (x.Field == \"term\").Fuzzy(0.6f)");

            if (query.QueryType != QueryType.Default)
                throw new NotSupportedException(
                    "Fuzzy in only supported with default queries. Example: (x.Field == \"term\")");

            query.Fuzzy = GetFuzzy(expression);

            return query;
        }

        private static float GetFuzzy(MethodCallExpression expression) {
            return (float) ((ConstantExpression) expression.Arguments[1]).Value;
        }
    }
}
