/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq {
    internal interface ISearcherHandle : IDisposable {
        IndexSearcher Searcher { get; }
    }
}
