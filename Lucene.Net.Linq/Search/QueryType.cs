/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

namespace Sprockets.Lucene.Net.Linq.Search {
    public enum QueryType {
        Default,
        Prefix,
        Suffix,
        Wildcard,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }
}
