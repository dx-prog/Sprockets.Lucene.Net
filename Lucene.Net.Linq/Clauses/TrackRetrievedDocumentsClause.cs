/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;

namespace Sprockets.Lucene.Net.Linq.Clauses {
    internal class TrackRetrievedDocumentsClause : ExtensionClause<ConstantExpression> {
        public TrackRetrievedDocumentsClause(ConstantExpression expression)
            : base(expression) {
        }

        public ConstantExpression Tracker => expression;

        public override IBodyClause Clone(CloneContext cloneContext) {
            return new TrackRetrievedDocumentsClause(expression);
        }

        protected override void Accept(ILuceneQueryModelVisitor visitor, QueryModel queryModel, int index) {
            visitor.VisitTrackRetrievedDocumentsClause(this, queryModel, index);
        }
    }
}
