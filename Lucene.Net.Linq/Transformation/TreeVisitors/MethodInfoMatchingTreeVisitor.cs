/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq.Parsing;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    internal abstract class MethodInfoMatchingTreeVisitor : RelinqExpressionVisitor {
        private readonly HashSet<MethodInfo> methods = new HashSet<MethodInfo>();

        protected override Expression VisitMethodCall(MethodCallExpression expression) {
            var method = expression.Method.IsGenericMethod
                ? expression.Method.GetGenericMethodDefinition()
                : expression.Method;

            if (!methods.Contains(method))
                return base.VisitMethodCall(expression);

            return VisitSupportedMethodCallExpression(expression);
        }

        protected abstract Expression VisitSupportedMethodCallExpression(MethodCallExpression expression);

        internal void AddMethod(MethodInfo method) {
            methods.Add(method.IsGenericMethod ? method.GetGenericMethodDefinition() : method);
        }
    }
}
