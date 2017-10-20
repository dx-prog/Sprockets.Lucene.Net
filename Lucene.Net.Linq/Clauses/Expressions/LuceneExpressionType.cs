/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

namespace Sprockets.Lucene.Net.Linq.Clauses.Expressions {
    internal enum LuceneExpressionType {
        LuceneQueryFieldExpression = 150001,
        LuceneQueryPredicateExpression,
        LuceneRangeQueryExpression,
        LuceneCompositeOrderingExpression,
        LuceneOrderByRelevanceExpression,
        LuceneQueryAnyFieldExpression,
        BoostBinaryExpression,
        LuceneQueryExpression,
        AllowSpecialCharactersExpression
    }
}
