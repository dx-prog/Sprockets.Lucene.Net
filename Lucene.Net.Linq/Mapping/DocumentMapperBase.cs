/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Analysis;
using Version = Lucene.Net.Util.Version;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    public abstract class DocumentMapperBase<T> : IDocumentMapper<T>, IDocumentKeyConverter,
        IDocumentModificationDetector<T> {
        protected readonly Analyzer externalAnalyzer;

        protected readonly IDictionary<string, IFieldMapper<T>> fieldMap =
            new Dictionary<string, IFieldMapper<T>>(StringComparer.Ordinal);

        protected readonly List<IFieldMapper<T>> keyFields = new List<IFieldMapper<T>>();
        protected readonly Version version;
        protected PerFieldAnalyzer analyzer;

        /// <summary>
        ///     Constructs an instance that will create an <see cref="Analyzer" />
        ///     using metadata on public properties on the type <typeparamref name="T" />.
        /// </summary>
        /// <param name="version">Version compatibility for analyzers and indexers.</param>
        protected DocumentMapperBase(Version version)
            : this(version, null) {
        }

        /// <summary>
        ///     Constructs an instance with an externall supplied analyzer
        ///     and the compatibility version of the index.
        /// </summary>
        /// <param name="version">Version compatibility for analyzers and indexers.</param>
        /// <param name="externalAnalyzer"></param>
        protected DocumentMapperBase(Version version, Analyzer externalAnalyzer) {
            this.version = version;
            this.externalAnalyzer = externalAnalyzer;
            analyzer = new PerFieldAnalyzer(new KeywordAnalyzer());
        }

        protected virtual bool EnableScoreTracking {
            get { return fieldMap.Values.Any(m => m is ReflectionScoreMapper<T>); }
        }

        public virtual IDocumentKey ToKey(Document document) {
            var keyValues = keyFields.ToDictionary(f => (IFieldMappingInfo) f, f => GetFieldValue(f, document));

            ValidateKey(keyValues);

            return new DocumentKey(keyValues);
        }

        public virtual PerFieldAnalyzer Analyzer => analyzer;

        public virtual IEnumerable<string> AllProperties {
            get { return fieldMap.Values.Select(m => m.PropertyName); }
        }

        public IEnumerable<string> IndexedProperties {
            get { return fieldMap.Values.Where(m => m.IndexMode != IndexMode.NotIndexed).Select(m => m.PropertyName); }
        }

        public virtual IEnumerable<string> KeyProperties {
            get { return keyFields.Select(k => k.PropertyName); }
        }

        public virtual IFieldMappingInfo GetMappingInfo(string propertyName) {
            return fieldMap[propertyName];
        }

        public virtual void ToObject(Document source, IQueryExecutionContext context, T target) {
            foreach (var mapping in fieldMap)
                mapping.Value.CopyFromDocument(source, context, target);
        }

        public virtual void ToDocument(T source, Document target) {
            foreach (var mapping in fieldMap)
                mapping.Value.CopyToDocument(source, target);
        }

        public virtual IDocumentKey ToKey(T source) {
            var keyValues = keyFields.ToDictionary(f => (IFieldMappingInfo) f, f => f.GetPropertyValue(source));

            ValidateKey(keyValues);

            return new DocumentKey(keyValues);
        }

        public virtual void PrepareSearchSettings(IQueryExecutionContext context) {
            if (EnableScoreTracking)
                context.Searcher.SetDefaultFieldSortScoring(true, false);
        }

        public Query CreateMultiFieldQuery(string pattern) {
            // TODO: pattern should be analyzed/converted on per-field basis.
            var parser = new MultiFieldQueryParser(version, fieldMap.Keys.ToArray(), externalAnalyzer);
            return parser.Parse(pattern);
        }

        public virtual bool Equals(T item1, T item2) {
            foreach (var field in fieldMap.Values) {
                var val1 = field.GetPropertyValue(item1);
                var val2 = field.GetPropertyValue(item2);

                if (!ValuesEqual(val1, val2))
                    return false;
            }

            return true;
        }

        public virtual bool IsModified(T item, Document document) {
            foreach (var field in fieldMap.Values) {
                // IFieldMapper should tell us if the field is transient/non-comparable
                if (field is ReflectionScoreMapper<T>)
                    continue;

                var val1 = field.GetPropertyValue(item);
                var val2 = GetFieldValue(field, document);

                if (!ValuesEqual(val1, val2))
                    return true;
            }

            return false;
        }

        public void AddField(IFieldMapper<T> fieldMapper) {
            fieldMap.Add(fieldMapper.PropertyName, fieldMapper);
            if (!string.IsNullOrWhiteSpace(fieldMapper.FieldName) && fieldMapper.Analyzer != null)
                Analyzer.AddAnalyzer(fieldMapper.FieldName, fieldMapper.Analyzer);
        }

        public void AddKeyField(IFieldMapper<T> fieldMapper) {
            AddField(fieldMapper);
            keyFields.Add(fieldMapper);
        }

        protected internal virtual bool ValuesEqual(object val1, object val2) {
            if (val1 is IEnumerable && val2 is IEnumerable)
                return ((IEnumerable) val1).Cast<object>().SequenceEqual(((IEnumerable) val2).Cast<object>());

            return Equals(val1, val2);
        }

        protected virtual void ValidateKey(Dictionary<IFieldMappingInfo, object> keyValues) {
            var nulls = keyValues.Where(kv => kv.Value == null).ToArray();

            if (!nulls.Any())
                return;

            var message = string.Format(
                "Cannot create key for document of type '{0}' with null value(s) for properties {1} which are marked as Key=true.",
                typeof(T),
                string.Join(", ", nulls.Select(n => n.Key.PropertyName)));

            throw new InvalidOperationException(message);
        }

        private object GetFieldValue(IFieldMappingInfo fieldMapper, Document document) {
            var fieldConverter = fieldMapper as IDocumentFieldConverter;

            if (fieldConverter == null)
                throw new NotSupportedException(
                    string.Format("The field mapping of type {0} for field {1} must implement {2}.",
                        fieldMapper.GetType(),
                        fieldMapper.FieldName,
                        typeof(IDocumentFieldConverter)));

            return fieldConverter.GetFieldValue(document);
        }
    }
}
