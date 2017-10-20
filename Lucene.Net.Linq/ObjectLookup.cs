/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Documents;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq {
    /// <summary>
    ///     Delegate used by <see cref="LuceneDataProvider" /> to create or reuse instances of objects
    ///     for advanced clients that may wish to provide caching.
    /// </summary>
    /// <typeparam name="T">The type of object <see cref="Document" />s will be mapped onto.</typeparam>
    /// <param name="key">A key that uniquely identifies the <see cref="Document" />.</param>
    /// <returns>An instance of <paramref name="T" /></returns>
    public delegate T ObjectLookup<out T>(IDocumentKey key);
}
