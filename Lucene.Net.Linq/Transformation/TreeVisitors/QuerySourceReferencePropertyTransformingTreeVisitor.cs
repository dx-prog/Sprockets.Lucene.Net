/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces MemberExpression instances like [QuerySourceReferenceExpression].PropertyName with
    ///     <c ref="LuceneQueryFieldExpression" />
    /// </summary>
    internal class QuerySourceReferencePropertyTransformingTreeVisitor : NoOpConvertExpressionRemovingVisitor {
        private MemberExpression parent;
        private LuceneQueryFieldExpression queryField;

        protected override Expression VisitMember(MemberExpression expression) {
            parent = expression;

            var result = base.VisitMember(expression);

            return queryField ?? result;
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression) {
            var propertyInfo = parent.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new NotSupportedException(
                    "Only MemberExpression of type PropertyInfo may be used on QuerySourceReferenceExpression.");

            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsEnum)
                propertyType = Enum.GetUnderlyingType(propertyType);

            queryField = new LuceneQueryFieldExpression(propertyType, propertyInfo.Name);
            return base.VisitQuerySourceReference(expression);
        }
    }
}
