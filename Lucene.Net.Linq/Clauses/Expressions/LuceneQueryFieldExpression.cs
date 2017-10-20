/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneQueryFieldExpression : LuceneExtensionExpression {
        internal LuceneQueryFieldExpression(Type type, string fieldName)
            : base(type, (ExpressionType) LuceneExpressionType.LuceneQueryFieldExpression) {
            FieldName = fieldName;
            Boost = 1;
        }

        internal LuceneQueryFieldExpression(Type type, ExpressionType expressionType, string fieldName)
            : base(type, expressionType) {
            FieldName = fieldName;
        }

        public string FieldName { get; }

        public float Boost { get; set; }

        public bool Equals(LuceneQueryFieldExpression other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Equals(other.FieldName, FieldName);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(LuceneQueryFieldExpression))
                return false;

            return Equals((LuceneQueryFieldExpression) obj);
        }

        public override int GetHashCode() {
            return FieldName != null ? FieldName.GetHashCode() : 0;
        }

        public static bool operator ==(LuceneQueryFieldExpression left, LuceneQueryFieldExpression right) {
            return Equals(left, right);
        }

        public static bool operator !=(LuceneQueryFieldExpression left, LuceneQueryFieldExpression right) {
            return !Equals(left, right);
        }

        public override string ToString() {
            var s = "LuceneField(" + FieldName + ")";
            if (Math.Abs(Boost - 1.0f) > 0.01f)
                return s + "^" + Boost;

            return s;
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            // no children.
            return this;
        }
    }
}
