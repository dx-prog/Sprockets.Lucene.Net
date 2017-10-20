/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Sprockets.Lucene.Net.Linq.Clauses.ExpressionNodes {
    internal class BoostExpressionNode : MethodCallExpressionNodeBase {
        public static readonly MethodInfo[] SupportedMethods = {
            ReflectionUtility.GetGenericMethod(() => LuceneMethods.BoostInternal<object>(null, null))
        };

        private readonly LambdaExpression _boostFunction;

        public BoostExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression boostFunction) :
            base(parseInfo) {
            _boostFunction = boostFunction;
        }

        public override Expression Resolve(ParameterExpression inputParameter,
            Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext) {
            return Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel,
            ClauseGenerationContext clauseGenerationContext) {
            queryModel.BodyClauses.Add(new BoostClause(_boostFunction));
        }
    }
}
