/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Search;
using Sprockets.Lucene.Net.Linq.Util;
using Version = Lucene.Net.Util.Version;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    public class ReflectionFieldMapper<T> : IFieldMapper<T>, IDocumentFieldConverter {
        protected static ConcurrentDictionary<string, object> internalCache =
            new ConcurrentDictionary<string, object>(StringComparer.Ordinal);

        protected readonly Analyzer analyzer;
        protected readonly float boost;
        protected readonly bool caseSensitive;
        protected readonly TypeConverter converter;
        protected readonly QueryParser.Operator defaultParserOperator;
        protected readonly string fieldName;
        protected readonly IndexMode index;
        protected readonly bool nativeSort;
        protected readonly Func<T, object> propertyGetter;
        protected readonly PropertyInfo propertyInfo;
        protected readonly Action<T, object> propertySetter;
        protected readonly StoreMode store;
        protected readonly TermVectorMode termVector;

        public ReflectionFieldMapper(PropertyInfo propertyInfo,
            StoreMode store,
            IndexMode index,
            TermVectorMode termVector,
            TypeConverter converter,
            string fieldName,
            bool caseSensitive,
            Analyzer analyzer)
            : this(propertyInfo, store, index, termVector, converter, fieldName, caseSensitive, analyzer, 1f) {
        }

        public ReflectionFieldMapper(PropertyInfo propertyInfo,
            StoreMode store,
            IndexMode index,
            TermVectorMode termVector,
            TypeConverter converter,
            string fieldName,
            bool caseSensitive,
            Analyzer analyzer,
            float boost)
            : this(propertyInfo,
                store,
                index,
                termVector,
                converter,
                fieldName,
                QueryParser.Operator.OR,
                caseSensitive,
                analyzer,
                boost) {
        }

        public ReflectionFieldMapper(PropertyInfo propertyInfo,
            StoreMode store,
            IndexMode index,
            TermVectorMode termVector,
            TypeConverter converter,
            string fieldName,
            QueryParser.Operator defaultParserOperator,
            bool caseSensitive,
            Analyzer analyzer,
            float boost,
            bool nativeSort = false) {
            this.propertyInfo = propertyInfo;
            propertyGetter = CreatePropertyGetter(propertyInfo);
            if (propertyInfo.CanWrite)
                propertySetter = CreatePropertySetter(propertyInfo);
            this.store = store;
            this.index = index;
            this.termVector = termVector;
            this.converter = converter;
            this.fieldName = fieldName;
            this.defaultParserOperator = defaultParserOperator;
            this.caseSensitive = caseSensitive;
            this.analyzer = analyzer;
            this.boost = boost;
            this.nativeSort = nativeSort;
        }

        public virtual PropertyInfo PropertyInfo => propertyInfo;

        public virtual StoreMode Store => store;

        public virtual TermVectorMode TermVector => termVector;

        public virtual TypeConverter Converter => converter;

        public virtual bool CaseSensitive => caseSensitive;

        public virtual float Boost => boost;

        public virtual QueryParser.Operator DefaultParseOperator => defaultParserOperator;

        public virtual bool NativeSort => nativeSort;

        protected Field.Store FieldStore => (Field.Store) store;

        public object GetFieldValue(Document document) {
            var field = document.GetFieldable(fieldName);

            if (field == null)
                return null;

            if (!propertyInfo.CanWrite)
                return null;

            return ConvertFieldValue(field);
        }

        public virtual Analyzer Analyzer => analyzer;

        public virtual IndexMode IndexMode => index;

        public virtual string FieldName => fieldName;

        public virtual string PropertyName => propertyInfo.Name;

        public virtual object GetPropertyValue(T source) {
            return propertyGetter(source);
        }

        public virtual void CopyFromDocument(Document source, IQueryExecutionContext context, T target) {
            if (!propertyInfo.CanWrite)
                return;

            var fieldValue = GetFieldValue(source);

            if (fieldValue != null)
                propertySetter(target, fieldValue);
        }

        public virtual void CopyToDocument(T source, Document target) {
            var value = propertyGetter(source);

            target.RemoveFields(fieldName);

            AddField(target, value);
        }

        public virtual string ConvertToQueryExpression(object value) {
            if (converter != null)
                return (string) converter.ConvertTo(value, typeof(string));

            return (string) value;
        }

        public virtual string EscapeSpecialCharacters(string value) {
            return QueryParser.Escape(value ?? string.Empty);
        }

        public virtual Query CreateQuery(string pattern) {
            Query query;

            if (TryParseKeywordContainingWhitespace(pattern, out query))
                return query;

            var queryParser = new QueryParser(Version.LUCENE_30, FieldName, analyzer) {
                AllowLeadingWildcard = true,
                LowercaseExpandedTerms = !CaseSensitive,
                DefaultOperator = defaultParserOperator
            };

            return queryParser.Parse(pattern);
        }

        public virtual Query CreateRangeQuery(object lowerBound,
            object upperBound,
            RangeType lowerRange,
            RangeType upperRange) {
            var minInclusive = lowerRange == RangeType.Inclusive;
            var maxInclusive = upperRange == RangeType.Inclusive;

            var lowerBoundStr = lowerBound == null ? null : EvaluateExpressionToStringAndAnalyze(lowerBound);
            var upperBoundStr = upperBound == null ? null : EvaluateExpressionToStringAndAnalyze(upperBound);
            return new TermRangeQuery(FieldName, lowerBoundStr, upperBoundStr, minInclusive, maxInclusive);
        }

        public virtual SortField CreateSortField(bool reverse) {
            if (Converter == null || NativeSort)
                return new SortField(FieldName, SortField.STRING, reverse);

            var propertyType = propertyInfo.PropertyType;

            FieldComparatorSource source;

            if (typeof(IComparable).IsAssignableFrom(propertyType))
                source = new NonGenericConvertableFieldComparatorSource(propertyType, Converter);
            else if (typeof(IComparable<>).MakeGenericType(propertyType).IsAssignableFrom(propertyType))
                source = new GenericConvertableFieldComparatorSource(propertyType, Converter);
            else
                throw new NotSupportedException(string.Format(
                    "The type {0} does not implement IComparable or IComparable<T>. To use alphanumeric sorting, specify NativeSort=true on the mapping.",
                    propertyType));

            return new SortField(FieldName, source, reverse);
        }

        protected internal virtual object ConvertFieldValue(IFieldable field) {
            var fieldValue = (object) field.StringValue;

            if (converter != null)
                fieldValue = converter.ConvertFrom(fieldValue);
            return fieldValue;
        }

        protected internal void AddField(Document target, object value) {
            if (value == null)
                return;

            var fieldValue = (string) null;

            if (converter != null)
                fieldValue = (string) converter.ConvertTo(value, typeof(string));
            else if (value is string)
                fieldValue = (string) value;

            if (fieldValue != null) {
                var field = new Field(fieldName,
                    fieldValue,
                    FieldStore,
                    (Field.Index) index,
                    (Field.TermVector) TermVector);
                field.Boost = Boost;
                target.Add(field);
            }
        }

        /// <summary>
        ///     Attempt to determine if a given query pattern contains whitespace and
        ///     the analyzer does not tokenize on whitespace. This is a work-around
        ///     for cases when QueryParser would split a keyword that contains whitespace
        ///     into multiple tokens.
        /// </summary>
        protected virtual bool TryParseKeywordContainingWhitespace(string pattern, out Query query) {
            query = null;

            if (pattern.IndexOfAny(new[] {' ', '\t', '\r', '\n'}) < 0)
                return false;

            var terms = Analyzer.GetTerms(FieldName, pattern).ToList();

            if (terms.Count > 1)
                return false;

            var termValue = Unescape(terms.Single());
            var term = new Term(FieldName, termValue);

            if (IsWildcardPattern(termValue))
                query = new WildcardQuery(term);
            else
                query = new TermQuery(term);

            return true;
        }

        /// <summary>
        ///     Determine if a (potentially escaped) pattern contains
        ///     any non-escaped wildcard characters such as <c>*</c> or <c>?</c>.
        /// </summary>
        protected virtual bool IsWildcardPattern(string pattern) {
            var unescaped = pattern.Replace(@"\\", "");
            return unescaped.Replace(@"\*", "").Contains("*")
                   || unescaped.Replace(@"\?", "").Contains("?");
        }

        /// <summary>
        ///     Remove escape characters from a pattern. This method
        ///     is called when a <see cref="Query" /> is being created without using
        ///     <see cref="QueryParser.Parse" />.
        /// </summary>
        protected virtual string Unescape(string pattern) {
            return pattern.Replace(@"\", "");
        }

        /// <summary>
        ///     Creates a property getter method with Lambda Expressions.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        private static Func<T, object> CreatePropertyGetter(PropertyInfo propertyInfo) {
            // check cache to avoid creating another method
            var cacheKey = "getter." + propertyInfo.GetHashCode();
            object cache;
            if (internalCache.TryGetValue(cacheKey, out cache))
                return (Func<T, object>) cache;

            // create method
            var name = propertyInfo.Name;
            var source = Expression.Parameter(typeof(T));
            var method = Expression
                .Lambda<Func<T, object>>(Expression.Convert(Expression.Property(source, name), typeof(object)), source)
                .Compile();

            // add to cache and return
            internalCache.TryAdd(cacheKey, method);
            return method;
        }

        /// <summary>
        ///     Creates a property setter method with Lambda Expressions.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        private static Action<T, object> CreatePropertySetter(PropertyInfo propertyInfo) {
            // check cache to avoid creating another method
            var cacheKey = "setter." + propertyInfo.GetHashCode();
            object cache;
            if (internalCache.TryGetValue(cacheKey, out cache))
                return (Action<T, object>) cache;

            // create method
            var name = propertyInfo.Name;
            var propType = propertyInfo.PropertyType;

            var sourceType = Expression.Parameter(typeof(T));
            var argument = Expression.Parameter(typeof(object), name);
            var propExp = Expression.Property(sourceType, name);

            var castToObject = Expression.Convert(argument, propType);

            var method = Expression
                .Lambda<Action<T, object>>(Expression.Assign(propExp, castToObject), sourceType, argument)
                .Compile();

            // add to cache and return
            internalCache.TryAdd(cacheKey, method);
            return method;
        }

        private string EvaluateExpressionToStringAndAnalyze(object value) {
            return analyzer.Analyze(FieldName, ConvertToQueryExpression(value));
        }
    }
}
