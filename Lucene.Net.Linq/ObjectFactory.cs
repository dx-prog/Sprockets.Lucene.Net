/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using Lucene.Net.Documents;

namespace Sprockets.Lucene.Net.Linq {
    /// <summary>
    ///     Delegate used by <see cref="LuceneDataProvider" /> to create instances of objects that do not
    ///     have public default constructors.
    /// </summary>
    /// <typeparam name="T">The type of object <see cref="Document" />s will be mapped onto.</typeparam>
    /// <returns>An instance of <paramref name="T" /></returns>
    public delegate T ObjectFactory<out T>();
}
