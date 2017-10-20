/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq.Parsing;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;

namespace Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors {
    internal class AllowSpecialCharactersMethodExpressionTreeVisitor : RelinqExpressionVisitor {
        private bool allowed;
        private LuceneQueryPredicateExpression parent;

        protected override Expression VisitExtension(Expression expression) {
            if (expression is AllowSpecialCharactersExpression)
                return VisitAllowSpecialCharactersExpression((AllowSpecialCharactersExpression) expression);

            if (expression is LuceneQueryPredicateExpression)
                return VisitQueryPredicateExpression((LuceneQueryPredicateExpression) expression);

            return base.VisitExtension(expression);
        }

        private Expression VisitAllowSpecialCharactersExpression(AllowSpecialCharactersExpression expression) {
            allowed = true;

            if (parent != null)
                parent.AllowSpecialCharacters = true;

            var result = Visit(expression.Pattern);

            allowed = false;

            return result;
        }

        private Expression VisitQueryPredicateExpression(LuceneQueryPredicateExpression expression) {
            parent = expression;

            var result = base.VisitExtension(expression);

            if (allowed && result is LuceneQueryPredicateExpression)
                ((LuceneQueryPredicateExpression) result).AllowSpecialCharacters = true;

            parent = null;

            return result;
        }
    }
}
