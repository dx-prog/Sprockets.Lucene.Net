/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal class LuceneCompositeOrderingExpression : LuceneExtensionExpression {
        public LuceneCompositeOrderingExpression(IEnumerable<LuceneQueryFieldExpression> fields)
            : base(typeof(object), (ExpressionType) LuceneExpressionType.LuceneCompositeOrderingExpression) {
            Fields = fields;
        }

        public IEnumerable<LuceneQueryFieldExpression> Fields { get; }

        protected override Expression VisitChildren(ExpressionVisitor visitor) {
            return this;
        }
    }
}
