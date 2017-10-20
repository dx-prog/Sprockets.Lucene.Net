/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Documents;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq {
    internal interface IRetrievedDocumentTracker<T> {
        void TrackDocument(IDocumentKey key, T item, Document document);
        bool TryGetTrackedDocument(IDocumentKey key, out T tracked);
        bool IsMarkedForDeletion(IDocumentKey key);
    }
}
