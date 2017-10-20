/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using System.Collections.Generic;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     Represents a unique key for a document
    /// </summary>
    public interface IDocumentKey : IEquatable<IDocumentKey> {
        /// <summary>
        ///     Flag indicating if the key is empty, meaning
        ///     that no key fields are defined for the document.
        /// </summary>
        bool Empty { get; }

        /// <summary>
        ///     Contains list of properties that are used for the key.
        /// </summary>
        IEnumerable<string> Properties { get; }

        /// <summary>
        ///     Retrieves the value for a given property.
        /// </summary>
        object this[string property] { get; }

        /// <summary>
        ///     Converts the key to a Lucene.Net <see cref="Query" />
        ///     that will match a unique document in the index.
        /// </summary>
        Query ToQuery();
    }
}
