/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Documents;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     <para>
    ///         Determines if an item has been modified for the purpose
    ///         of writing modified documents within an <see cref="ISession{T}" />
    ///     </para>
    ///     <para>Since 3.2</para>
    /// </summary>
    public interface IDocumentModificationDetector<in T> {
        /// <summary>
        ///     Determine if an item is modified by comparing
        ///     the properties to the fields on the document
        ///     from which the item was initialized.
        ///     This method replaces <see cref="IDocumentMapper{T}.Equals(T,T)" />
        /// </summary>
        bool IsModified(T item, Document document);
    }
}
