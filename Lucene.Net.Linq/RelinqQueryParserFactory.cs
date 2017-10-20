/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using Sprockets.Lucene.Net.Linq.Clauses.ExpressionNodes;
using Sprockets.Lucene.Net.Linq.Transformation;

namespace Sprockets.Lucene.Net.Linq {
    internal static class RelinqQueryParserFactory {
        internal static QueryParser CreateQueryParser() {
            var expressionTreeParser = new ExpressionTreeParser(
                CreateNodeTypeProvider(),
                CreateExpressionTreeProcessor());

            return new QueryParser(expressionTreeParser);
        }

        /// <summary>
        ///     Creates an <c cref="IExpressionTreeProcessor" /> that will execute
        ///     <c cref="AllowSpecialCharactersExpressionTransformer" />
        ///     before executing <c cref="PartialEvaluatingExpressionTreeProcessor" />
        ///     and other default processors.
        /// </summary>
        internal static IExpressionTreeProcessor CreateExpressionTreeProcessor() {
            var firstRegistry = new ExpressionTransformerRegistry();
            firstRegistry.Register(new AllowSpecialCharactersExpressionTransformer());

            var processor = ExpressionTreeParser.CreateDefaultProcessor(ExpressionTransformerRegistry.CreateDefault());
            processor.InnerProcessors.Insert(0, new TransformingExpressionTreeProcessor(firstRegistry));
            return processor;
        }

        private static CompoundNodeTypeProvider CreateNodeTypeProvider() {
            var a = new MethodInfoBasedNodeTypeRegistry();
            a.Register(BoostExpressionNode.SupportedMethods, typeof(BoostExpressionNode));
            a.Register(TrackRetrievedDocumentsExpressionNode.SupportedMethods,
                typeof(TrackRetrievedDocumentsExpressionNode));
            a.Register(QueryStatisticsCallbackExpressionNode.SupportedMethods,
                typeof(QueryStatisticsCallbackExpressionNode));

            var nodeTypeProvider = ExpressionTreeParser.CreateDefaultNodeTypeProvider();
            nodeTypeProvider.InnerProviders.Add(a);

            return nodeTypeProvider;
        }
    }
}
