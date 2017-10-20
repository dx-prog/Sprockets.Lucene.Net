/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneQueryExpression : LuceneExtensionExpression {
        internal LuceneQueryExpression(Query query)
            : base(typeof(Query), (ExpressionType) LuceneExpressionType.LuceneQueryExpression) {
            Query = query;
        }

        public Query Query { get; }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            // no children.
            return this;
        }
    }
}
