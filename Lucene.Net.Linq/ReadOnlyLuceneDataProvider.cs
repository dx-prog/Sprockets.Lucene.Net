/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using Lucene.Net.Analysis;
using Lucene.Net.Store;
using Sprockets.Lucene.Net.Linq.Abstractions;
using Sprockets.Lucene.Net.Linq.Mapping;
using Version = Lucene.Net.Util.Version;

namespace Sprockets.Lucene.Net.Linq {
    public class ReadOnlyLuceneDataProvider : LuceneDataProvider {
        public ReadOnlyLuceneDataProvider(Directory directory, Version version) : base(directory, version) {
        }

        public ReadOnlyLuceneDataProvider(Directory directory, Analyzer externalAnalyzer, Version version) : base(
            directory,
            externalAnalyzer,
            version) {
        }

        public override ISession<T> OpenSession<T>(ObjectLookup<T> factory,
            IDocumentMapper<T> documentMapper,
            IDocumentModificationDetector<T> documentModificationDetector) {
            throw new InvalidOperationException("Cannot open sessions in read-only mode.");
        }

        protected override IIndexWriter GetIndexWriter(Analyzer analyzer) {
            return null;
        }
    }
}
