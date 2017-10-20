/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    internal class DocumentKeyFieldMapper<T> : IFieldMapper<T>, IDocumentFieldConverter {
        private readonly string value;

        public DocumentKeyFieldMapper(string fieldName, string value) {
            FieldName = fieldName;
            this.value = value;
        }

        public object GetFieldValue(Document document) {
            return value;
        }

        public object GetPropertyValue(T source) {
            return value;
        }

        public void CopyToDocument(T source, Document target) {
            target.Add(new Field(FieldName, value, Field.Store.YES, Field.Index.NOT_ANALYZED));
        }

        public void CopyFromDocument(Document source, IQueryExecutionContext context, T target) {
        }

        public string ConvertToQueryExpression(object value) {
            return this.value;
        }

        public string EscapeSpecialCharacters(string value) {
            return QueryParser.Escape(value ?? string.Empty);
        }

        public Query CreateRangeQuery(object lowerBound,
            object upperBound,
            RangeType lowerRange,
            RangeType upperRange) {
            throw new NotSupportedException();
        }

        public Query CreateQuery(string ignored) {
            return new TermQuery(new Term(FieldName, value));
        }

        public SortField CreateSortField(bool reverse) {
            throw new NotSupportedException();
        }

        public string PropertyName => "**DocumentKey**" + FieldName;

        public string FieldName { get; }

        public Analyzer Analyzer => new KeywordAnalyzer();

        public IndexMode IndexMode => IndexMode.NotAnalyzed;
    }
}
