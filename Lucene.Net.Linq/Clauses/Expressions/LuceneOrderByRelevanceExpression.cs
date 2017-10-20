/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneOrderByRelevanceExpression : LuceneExtensionExpression {
        private static readonly LuceneOrderByRelevanceExpression instance = new LuceneOrderByRelevanceExpression();

        private LuceneOrderByRelevanceExpression()
            : base(typeof(object), (ExpressionType) LuceneExpressionType.LuceneOrderByRelevanceExpression) {
        }

        public static Expression Instance => instance;

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            return this;
        }
    }
}
