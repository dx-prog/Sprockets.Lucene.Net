/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Util;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq.Fluent {
    internal class FluentDocumentMapper<T> : DocumentMapperBase<T> {
        public FluentDocumentMapper(Version version) : base(version) {
        }
    }
}
