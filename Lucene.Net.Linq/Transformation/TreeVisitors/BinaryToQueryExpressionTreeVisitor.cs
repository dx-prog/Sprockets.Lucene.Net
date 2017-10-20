/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Linq.Expressions;
using Lucene.Net.Search;
using Remotion.Linq.Parsing;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    internal class BinaryToQueryExpressionTreeVisitor : RelinqExpressionVisitor {
        protected override Expression VisitBinary(BinaryExpression expression) {
            QueryType queryType;

            if (!expression.NodeType.TryGetQueryType(out queryType))
                return base.VisitBinary(expression);

            var occur = Occur.MUST;
            if (expression.NodeType == ExpressionType.NotEqual)
                occur = Occur.MUST_NOT;

            LuceneQueryFieldExpression fieldExpression;
            Expression pattern;

            if (expression.Left is LuceneQueryFieldExpression) {
                fieldExpression = (LuceneQueryFieldExpression) expression.Left;
                pattern = expression.Right;
            }
            else if (expression.Right is LuceneQueryFieldExpression) {
                fieldExpression = (LuceneQueryFieldExpression) expression.Right;
                pattern = expression.Left;

                switch (queryType) {
                    case QueryType.GreaterThan:
                        queryType = QueryType.LessThan;
                        break;
                    case QueryType.LessThan:
                        queryType = QueryType.GreaterThan;
                        break;
                    case QueryType.GreaterThanOrEqual:
                        queryType = QueryType.LessThanOrEqual;
                        break;
                    case QueryType.LessThanOrEqual:
                        queryType = QueryType.GreaterThanOrEqual;
                        break;
                }
            }
            else {
                throw new NotSupportedException("Expected Left or Right to be LuceneQueryFieldExpression");
            }

            return new LuceneQueryPredicateExpression(fieldExpression, pattern, occur, queryType);
        }
    }
}
