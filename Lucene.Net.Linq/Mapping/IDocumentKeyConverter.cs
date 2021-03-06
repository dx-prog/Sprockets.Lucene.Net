/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Documents;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     <para>
    ///         Allows a <see cref="Document" /> to be converted
    ///         to an <see cref="IDocumentKey" /> without first
    ///         being mapped onto an object.
    ///     </para>
    ///     <para>Since 3.2</para>
    /// </summary>
    public interface IDocumentKeyConverter {
        /// <summary>
        ///     Create a composite key representing a unique
        ///     identity for the document. <see cref="IDocumentMapper{T}.ToKey" />
        /// </summary>
        IDocumentKey ToKey(Document document);
    }
}
