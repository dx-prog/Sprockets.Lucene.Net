/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq.Search {
    public static class ExpressionTypeExtentions {
        private static readonly IDictionary<ExpressionType, QueryType> typeMap =
            new Dictionary<ExpressionType, QueryType> {
                {ExpressionType.GreaterThan, QueryType.GreaterThan},
                {ExpressionType.GreaterThanOrEqual, QueryType.GreaterThanOrEqual},
                {ExpressionType.LessThan, QueryType.LessThan},
                {ExpressionType.LessThanOrEqual, QueryType.LessThanOrEqual},
                {ExpressionType.Equal, QueryType.Default},
                {ExpressionType.NotEqual, QueryType.Default}
            };

        public static QueryType ToQueryType(this ExpressionType type) {
            return typeMap[type];
        }

        public static bool TryGetQueryType(this ExpressionType type, out QueryType queryType) {
            return typeMap.TryGetValue(type, out queryType);
        }
    }
}
