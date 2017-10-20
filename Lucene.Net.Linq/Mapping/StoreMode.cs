/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Documents;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <see cref="Field.Store" />
    public enum StoreMode {
        /// <see cref="Field.Store.YES" />
        Yes = Field.Store.YES,

        /// <see cref="Field.Store.NO" />
        No = Field.Store.NO
    }
}
