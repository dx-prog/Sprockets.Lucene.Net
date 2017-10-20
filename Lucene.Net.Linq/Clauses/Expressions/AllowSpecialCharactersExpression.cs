/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class AllowSpecialCharactersExpression : LuceneExtensionExpression {
        internal AllowSpecialCharactersExpression(Expression pattern)
            : base(pattern.Type, (ExpressionType) LuceneExpressionType.AllowSpecialCharactersExpression) {
            Pattern = pattern;
        }

        public Expression Pattern { get; }

        public override string ToString() {
            return Pattern + ".AllowSpecialCharacters()";
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            var newPattern = visitor.Visit(Pattern);

            if (Equals(Pattern, newPattern))
                return this;

            return new AllowSpecialCharactersExpression(newPattern);
        }
    }
}
