/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneRangeQueryExpression : LuceneExtensionExpression {
        public LuceneRangeQueryExpression(LuceneQueryFieldExpression field,
            Expression lower,
            QueryType lowerQueryType,
            Expression upper,
            QueryType upperQueryType)
            : this(field, lower, lowerQueryType, upper, upperQueryType, Occur.MUST) {
        }

        public LuceneRangeQueryExpression(LuceneQueryFieldExpression field,
            Expression lower,
            QueryType lowerQueryType,
            Expression upper,
            QueryType upperQueryType,
            Occur occur)
            : base(typeof(bool), (ExpressionType) LuceneExpressionType.LuceneRangeQueryExpression) {
            QueryField = field;
            Lower = lower;
            LowerQueryType = lowerQueryType;
            Upper = upper;
            UpperQueryType = upperQueryType;
            Occur = occur;
        }

        public LuceneQueryFieldExpression QueryField { get; }

        public Expression Lower { get; }

        public QueryType LowerQueryType { get; }

        public Expression Upper { get; }

        public QueryType UpperQueryType { get; }

        public Occur Occur { get; }

        public override string ToString() {
            return string.Format("{0}LuceneRangeQuery({1} {2} TO {3} {4}",
                Occur,
                LowerQueryType,
                Lower,
                UpperQueryType,
                Upper);
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            var newField = (LuceneQueryFieldExpression) visitor.Visit(QueryField);
            var newLower = visitor.Visit(Lower);
            var newUpper = visitor.Visit(Upper);

            return newField == QueryField && newLower == Lower && newUpper == Upper
                ? this
                : new LuceneRangeQueryExpression(newField, newLower, LowerQueryType, newUpper, UpperQueryType, Occur);
        }
    }
}
