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
    internal class ReflectionScoreMapper<T> : IFieldMapper<T>, IDocumentFieldConverter {
        private readonly PropertyInfo propertyInfo;

        public ReflectionScoreMapper(PropertyInfo propertyInfo) {
            this.propertyInfo = propertyInfo;
        }

        public object GetFieldValue(Document document) {
            return null;
        }

        public void CopyToDocument(T source, Document target) {
        }

        public void CopyFromDocument(Document source, IQueryExecutionContext context, T target) {
            if (context.Phase == QueryExecutionPhase.ConvertResults) {
                var score = context.CurrentScoreDoc.Score;
                propertyInfo.SetValue(target, score, null);
            }
        }

        public SortField CreateSortField(bool reverse) {
            if (reverse)
                return new SortField(SortField.FIELD_SCORE.Field, SortField.FIELD_SCORE.Type, true);

            return SortField.FIELD_SCORE;
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
            return 0;
        }

        public string PropertyName => propertyInfo.Name;
        public string FieldName => null;
        public Analyzer Analyzer => null;
        public IndexMode IndexMode => IndexMode.NotIndexed;
    }
}
