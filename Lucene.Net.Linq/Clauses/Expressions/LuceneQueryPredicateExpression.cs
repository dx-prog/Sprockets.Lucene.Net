/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneQueryPredicateExpression : LuceneExtensionExpression {
        public LuceneQueryPredicateExpression(LuceneQueryFieldExpression field, Expression pattern, Occur occur)
            : this(field, pattern, occur, QueryType.Default) {
        }

        public LuceneQueryPredicateExpression(LuceneQueryFieldExpression field,
            Expression pattern,
            Occur occur,
            QueryType queryType)
            : base(typeof(bool), (ExpressionType) LuceneExpressionType.LuceneQueryPredicateExpression) {
            QueryField = field;
            QueryPattern = pattern;
            Occur = occur;
            QueryType = queryType;
        }

        public LuceneQueryFieldExpression QueryField { get; }

        public Expression QueryPattern { get; }

        public Occur Occur { get; }

        public float Boost {
            get => QueryField.Boost;
            set => QueryField.Boost = value;
        }

        public float? Fuzzy { get; set; }

        public QueryType QueryType { get; }

        public bool AllowSpecialCharacters { get; set; }

        public override string ToString() {
            return string.Format("LuceneQuery[{0}]({1}{2}:{3}){4}{5}",
                QueryType,
                Occur,
                QueryField.FieldName,
                QueryPattern,
                Boost - 1.0f < 0.01f ? "" : "^" + Boost,
                AllowSpecialCharacters ? ".AllowSpecialCharacters()" : "");
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            var newField = (LuceneQueryFieldExpression) visitor.Visit(QueryField);
            var newPattern = visitor.Visit(QueryPattern);

            return newPattern == QueryPattern && newField == QueryField
                ? this
                : new LuceneQueryPredicateExpression(newField, newPattern, Occur) {
                    AllowSpecialCharacters = AllowSpecialCharacters
                };
        }
    }
}
