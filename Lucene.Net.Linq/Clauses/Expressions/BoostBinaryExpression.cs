/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class BoostBinaryExpression : LuceneExtensionExpression {
        public BoostBinaryExpression(BinaryExpression expression, float boost)
            : base(expression.Type, (ExpressionType) LuceneExpressionType.BoostBinaryExpression) {
            BinaryExpression = expression;
            Boost = boost;
        }

        public BinaryExpression BinaryExpression { get; }

        public float Boost { get; }

        public override string ToString() {
            return string.Format("{0}^{1}", BinaryExpression, Boost);
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            var newExpression = visitor.Visit(BinaryExpression);

            if (ReferenceEquals(BinaryExpression, newExpression))
                return this;

            return new BoostBinaryExpression((BinaryExpression) newExpression, Boost);
        }
    }
}
