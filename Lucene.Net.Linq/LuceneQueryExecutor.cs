/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq {
    internal class LuceneQueryExecutor<TDocument> : LuceneQueryExecutorBase<TDocument> {
        private readonly IDocumentKeyConverter keyConverter;
        private readonly IDocumentMapper<TDocument> mapper;
        private readonly ObjectLookup<TDocument> newItem;

        public LuceneQueryExecutor(Context context, ObjectLookup<TDocument> newItem, IDocumentMapper<TDocument> mapper)
            : base(context) {
            this.newItem = newItem;
            this.mapper = mapper;
            keyConverter = mapper as IDocumentKeyConverter;
        }

        public override IEnumerable<string> AllProperties => mapper.AllProperties;

        public override IEnumerable<string> IndexedProperties => mapper.IndexedProperties;

        public override IEnumerable<string> KeyProperties => mapper.KeyProperties;

        public override IFieldMappingInfo GetMappingInfo(string propertyName) {
            return mapper.GetMappingInfo(propertyName);
        }

        public override Query CreateMultiFieldQuery(string pattern) {
            return mapper.CreateMultiFieldQuery(pattern);
        }

        protected override TDocument ConvertDocument(Document doc, IQueryExecutionContext context) {
            var key = (IDocumentKey) null;
            if (keyConverter != null)
                key = keyConverter.ToKey(doc);

            var item = newItem(key);

            mapper.ToObject(doc, context, item);

            return item;
        }

        protected override TDocument ConvertDocumentForCustomBoost(Document doc) {
            return ConvertDocument(doc, new QueryExecutionContext());
        }

        protected override IDocumentKey GetDocumentKey(Document doc, IQueryExecutionContext context) {
            if (keyConverter != null)
                return keyConverter.ToKey(doc);

            var item = ConvertDocument(doc, context);

            return mapper.ToKey(item);
        }

        protected override void PrepareSearchSettings(IQueryExecutionContext context) {
            mapper.PrepareSearchSettings(context);
        }
    }
}
