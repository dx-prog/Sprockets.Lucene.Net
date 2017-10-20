/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Reflection;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Search;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    internal class ReflectionDocumentBoostMapper<T> : IFieldMapper<T>, IDocumentFieldConverter {
        private readonly PropertyInfo propertyInfo;

        public ReflectionDocumentBoostMapper(PropertyInfo propertyInfo) {
            this.propertyInfo = propertyInfo;
        }

        public object GetFieldValue(Document document) {
            return document.Boost;
        }

        public void CopyToDocument(T source, Document target) {
            target.Boost = (float) GetPropertyValue(source);
        }

        public void CopyFromDocument(Document source, IQueryExecutionContext context, T target) {
            var value = GetFieldValue(source);

            propertyInfo.SetValue(target, value, null);
        }

        public SortField CreateSortField(bool reverse) {
            throw new NotSupportedException();
        }

        public string ConvertToQueryExpression(object value) {
            throw new NotSupportedException();
        }

        public string EscapeSpecialCharacters(string value) {
            throw new NotSupportedException();
        }

        public Query CreateQuery(string pattern) {
            throw new NotSupportedException();
        }

        public Query CreateRangeQuery(object lowerBound,
            object upperBound,
            RangeType lowerRange,
            RangeType upperRange) {
            throw new NotSupportedException();
        }

        public object GetPropertyValue(T source) {
            return propertyInfo.GetValue(source, null);
        }

        public string PropertyName => propertyInfo.Name;
        public string FieldName => null;
        public Analyzer Analyzer => null;
        public IndexMode IndexMode => IndexMode.NotIndexed;
    }
}
