/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq.Expressions;
using Remotion.Linq.Parsing;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces expressions like <c>(bool)(Constant(bool?))</c> with <c>Constant(bool?)</c>.
    /// </summary>
    internal class NoOpConvertExpressionRemovingVisitor : RelinqExpressionVisitor {
        protected override Expression VisitBinary(BinaryExpression expression) {
            var left = Visit(expression.Left);
            var right = Visit(expression.Right);

            if (ReferenceEquals(left, expression.Left) && ReferenceEquals(right, expression.Right))
                return expression;

            left = ConvertIfNecessary(left, right.Type);
            right = ConvertIfNecessary(right, left.Type);

            return Expression.MakeBinary(expression.NodeType, left, right);
        }

        protected override Expression VisitUnary(UnaryExpression expression) {
            if (expression.NodeType == ExpressionType.Convert)
                return Visit(expression.Operand);

            var declaringType = expression.Type;
            if (declaringType.IsConstructedGenericType)
                if (declaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    expression = Expression.MakeUnary(expression.NodeType,
                        Visit(expression.Operand),
                        declaringType.GenericTypeArguments[0]);


            return base.VisitUnary(expression);
        }

        private Expression ConvertIfNecessary(Expression expression, Type type) {
            var constant = expression as ConstantExpression;
            if (constant == null || expression.Type == type)
                return expression;

            if (type.IsEnum)
                return Expression.Constant(Enum.ToObject(type, constant.Value));

            if (type.IsPrimitive)
                return Expression.Constant(Convert.ChangeType(constant.Value, type));

            return expression;
        }
    }
}
