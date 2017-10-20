/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sprockets.Lucene.Net.Linq {
    /// <summary>
    ///     Created from scatch by David Garcia 10-19-2017
    /// </summary>
    public static class ReflectionUtility {
        public static MethodInfo GetMethod(Expression<Action> call) {
            return ((MethodCallExpression) call.Body).Method;
        }

        public static MethodInfo GetGenericMethod(Expression<Action> call) {
            return ((MethodCallExpression) call.Body).Method.GetGenericMethodDefinition();
        }

        public static Expression VisitExpression(this ExpressionVisitor vistor, Expression expression) {
            return vistor.Visit(expression);
        }
    }
}
