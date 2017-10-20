/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Sprockets.Lucene.Net.Linq.Clauses.ExpressionNodes {
    internal class TrackRetrievedDocumentsExpressionNode : MethodCallExpressionNodeBase {
        public static readonly MethodInfo[] SupportedMethods = {
            ReflectionUtility.GetGenericMethod(() => LuceneMethods.TrackRetrievedDocuments<object>(null, null))
        };

        private readonly ConstantExpression tracker;

        public TrackRetrievedDocumentsExpressionNode(MethodCallExpressionParseInfo parseInfo,
            ConstantExpression tracker)
            : base(parseInfo) {
            this.tracker = tracker;
        }

        public override Expression Resolve(ParameterExpression inputParameter,
            Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext) {
            return Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel,
            ClauseGenerationContext clauseGenerationContext) {
            queryModel.BodyClauses.Add(new TrackRetrievedDocumentsClause(tracker));
        }
    }
}
