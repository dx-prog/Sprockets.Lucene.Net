/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.ComponentModel;
using System.Reflection;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Sprockets.Lucene.Net.Linq.Analysis;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq.Fluent {
    /// <summary>
    ///     A fluent interface for specifying additional options
    ///     for how a property is analyzed, indexed and stored.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyMap<T> {
        protected readonly ClassMap<T> classMap;
        protected readonly PropertyInfo propInfo;
        protected Analyzer analyzer;
        protected float boost = 1.0f;
        protected bool caseSensitive;
        protected TypeConverter converter;
        protected QueryParser.Operator defaultParseOperator = QueryParser.OR_OPERATOR;
        protected string fieldName;
        protected IndexMode indexMode = Mapping.IndexMode.Analyzed;
        protected bool isKey;
        protected bool nativeSort;
        protected StoreMode store = StoreMode.Yes;

        internal PropertyMap(ClassMap<T> classMap, PropertyInfo propInfo, bool isKey = false)
            : this(classMap, propInfo, null) {
            this.isKey = isKey;
        }

        protected internal PropertyMap(ClassMap<T> classMap, PropertyInfo propInfo, PropertyMap<T> copy) {
            this.classMap = classMap;
            this.propInfo = propInfo;
            SetDefaults(propInfo, copy);
        }

        /// <summary>
        ///     Controls whether term vectors are stored for later retrieval.
        ///     See <see cref="Field.TermVector" /> for more info.
        /// </summary>
        public TermVectorPart<T> WithTermVector => new TermVectorPart<T>(this);

        protected internal string PropertyName => propInfo.Name;

        protected internal bool IsKey => isKey;

        protected internal TermVectorMode TermVectorMode { get; set; }

        protected virtual Type PropertyType {
            get {
                Type type;

                FieldMappingInfoBuilder.IsCollection(propInfo.PropertyType, out type);

                return type;
            }
        }

        /// <summary>
        ///     Set the field name. Defaults to same as property name being mapped.
        /// </summary>
        public virtual PropertyMap<T> ToField(string fieldName) {
            this.fieldName = fieldName;
            return this;
        }

        /// <summary>
        ///     Configure values to be stored using <see cref="NumericField" /> instead
        ///     of default <see cref="Field" />.
        /// </summary>
        public NumericPropertyMap<T> AsNumericField() {
            if (this is NumericPropertyMap<T>)
                return (NumericPropertyMap<T>) this;

            var numericPart = new NumericPropertyMap<T>(classMap, propInfo, this);
            classMap.AddProperty(numericPart);
            return numericPart;
        }

        /// <summary>
        ///     Specify a custom TypeConverter to convert the given type to a <see cref="string" />
        ///     and back to the other <see cref="Type" />.
        /// </summary>
        public PropertyMap<T> ConvertWith(TypeConverter converter) {
            this.converter = converter;
            return this;
        }

        /// <summary>
        ///     Specify an <see cref="Analyzer" /> to use when indexing this property.
        /// </summary>
        public PropertyMap<T> AnalyzeWith(Analyzer analyzer) {
            this.analyzer = analyzer;
            return this;
        }

        /// <summary>
        ///     Specify that the field is stored for later retrieval (the default behavior).
        /// </summary>
        public PropertyMap<T> Stored() {
            store = StoreMode.Yes;
            return this;
        }

        /// <summary>
        ///     Specify that the field is NOT stored for later retrieval.
        /// </summary>
        public PropertyMap<T> NotStored() {
            store = StoreMode.No;
            return this;
        }

        /// <summary>
        ///     Specify a constant boost to apply to this field at indexing time.
        /// </summary>
        public PropertyMap<T> BoostBy(float amount) {
            boost = amount;
            return this;
        }

        /// <summary>
        ///     Specify that values for this field are case sensitive as
        ///     opposed to the default behavior which assumes that the
        ///     analyzer will convert tokens to lower case at indexing time.
        ///     This controls <see cref="QueryParser.LowercaseExpandedTerms" />
        ///     when building queries.
        /// </summary>
        public PropertyMap<T> CaseSensitive() {
            caseSensitive = true;
            return this;
        }

        /// <summary>
        ///     Set the <see cref="QueryParser.DefaultOperator" /> to
        ///     use <see cref="QueryParser.AND_OPERATOR" /> by default
        ///     when parsing queries that contain multiple terms.
        /// </summary>
        public PropertyMap<T> ParseWithAndOperatorByDefault() {
            defaultParseOperator = QueryParser.Operator.AND;
            return this;
        }

        /// <summary>
        ///     Set the <see cref="QueryParser.DefaultOperator" /> to
        ///     use <see cref="QueryParser.OR_OPERATOR" /> by default
        ///     when parsing queries that contain multiple terms. This
        ///     is the default behavior.
        /// </summary>
        public PropertyMap<T> ParseWithOrOperatorByDefault() {
            defaultParseOperator = QueryParser.Operator.OR;
            return this;
        }

        public PropertyMap<T> NativeSort() {
            nativeSort = true;
            return this;
        }

        protected internal virtual IFieldMapper<T> ToFieldMapper() {
            var mapper = ToFieldMapperInternal();

            Type type;

            if (FieldMappingInfoBuilder.IsCollection(propInfo.PropertyType, out type))
                return new CollectionReflectionFieldMapper<T>(mapper, type);

            return mapper;
        }

        protected internal virtual ReflectionFieldMapper<T> ToFieldMapperInternal() {
            return new ReflectionFieldMapper<T>(propInfo,
                store,
                indexMode,
                TermVectorMode,
                ResolveConverter(),
                fieldName,
                defaultParseOperator,
                caseSensitive,
                ResolveAnalyzer(),
                boost,
                nativeSort);
        }

        private TypeConverter ResolveConverter() {
            if (converter != null)
                return converter;

            var fakeAttr = new FieldAttribute(indexMode) {CaseSensitive = caseSensitive};

            return FieldMappingInfoBuilder.GetConverter(propInfo, PropertyType, fakeAttr);
        }

        private Analyzer ResolveAnalyzer() {
            if (analyzer != null)
                return analyzer;

            var fakeAttr = new FieldAttribute(indexMode) {CaseSensitive = caseSensitive};

            var flag = FieldMappingInfoBuilder.GetCaseSensitivity(fakeAttr, converter);

            return flag ? new KeywordAnalyzer() : new CaseInsensitiveKeywordAnalyzer();
        }

        private void SetDefaults(PropertyInfo propInfo, PropertyMap<T> copy) {
            if (copy != null) {
                isKey = copy.isKey;
                fieldName = copy.fieldName ?? propInfo.Name;
                return;
            }

            fieldName = propInfo.Name;
        }

        #region IndexMode settings

        /// <summary>
        ///     Specify IndexMode slightly less fluently.
        /// </summary>
        public PropertyMap<T> IndexMode(IndexMode mode) {
            indexMode = mode;
            return this;
        }

        /// <summary>
        ///     Specify IndexMode.
        /// </summary>
        public PropertyMap<T> Analyzed() {
            return IndexMode(Mapping.IndexMode.Analyzed);
        }

        /// <summary>
        ///     Specify IndexMode.
        /// </summary>
        public PropertyMap<T> AnalyzedNoNorms() {
            return IndexMode(Mapping.IndexMode.AnalyzedNoNorms);
        }

        /// <summary>
        ///     Specify IndexMode.
        /// </summary>
        public PropertyMap<T> NotAnalyzed() {
            return IndexMode(Mapping.IndexMode.NotAnalyzed);
        }

        /// <summary>
        ///     Specify IndexMode.
        /// </summary>
        public PropertyMap<T> NotAnalyzedNoNorms() {
            return IndexMode(Mapping.IndexMode.NotAnalyzedNoNorms);
        }

        /// <summary>
        ///     Specify IndexMode.
        /// </summary>
        public PropertyMap<T> NotIndexed() {
            return IndexMode(Mapping.IndexMode.NotIndexed);
        }

        #endregion
    }
}
