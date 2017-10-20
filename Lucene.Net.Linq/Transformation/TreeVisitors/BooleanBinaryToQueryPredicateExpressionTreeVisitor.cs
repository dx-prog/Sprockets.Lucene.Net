/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Lucene.Net.Search;
using Remotion.Linq.Parsing;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Util;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    /// <summary>
    ///     Replaces boolean binary expressions like <c>[LuceneQueryPredicateExpression](+field:query) == false</c> to
    ///     <c>[LuceneQueryPredicateExpression](-field:query)</c>
    /// </summary>
    internal class BooleanBinaryToQueryPredicateExpressionTreeVisitor : RelinqExpressionVisitor {
        protected override Expression VisitBinary(BinaryExpression expression) {
            var predicate = expression.Left as LuceneQueryPredicateExpression;

            var constant = expression.Right.IsTrueConstant();

            if (predicate == null || !(constant || expression.Right.IsFalseConstant()))
                return base.VisitBinary(expression);

            if (expression.NodeType == ExpressionType.Equal && constant ||
                expression.NodeType == ExpressionType.NotEqual && !constant)
                return predicate;

            return new LuceneQueryPredicateExpression(predicate.QueryField,
                    predicate.QueryPattern,
                    Occur.MUST_NOT,
                    predicate.QueryType)
                {Boost = predicate.Boost, AllowSpecialCharacters = predicate.AllowSpecialCharacters};
        }
    }
}
