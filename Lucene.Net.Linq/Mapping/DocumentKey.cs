/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Util;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    public class DocumentKey : IDocumentKey {
        private readonly IDictionary<string, IFieldMappingInfo> mappings;
        private readonly IDictionary<string, object> values;

        public DocumentKey() {
        }

        public DocumentKey(IDictionary<IFieldMappingInfo, object> values) {
            this.values = new SortedDictionary<string, object>(
                values.ToDictionary(kv => kv.Key.FieldName, kv => kv.Value, StringComparer.Ordinal),
                StringComparer.Ordinal);
            mappings = values.ToDictionary(kv => kv.Key.FieldName, kv => kv.Key, StringComparer.Ordinal);
        }

        public Query ToQuery() {
            if (Empty)
                throw new InvalidOperationException("No key fields defined.");

            var query = new BooleanQuery();
            values.Apply(kvp => query.Add(ConvertToQueryExpression(kvp), Occur.MUST));
            return query;
        }

        public IEnumerable<string> Properties => values.Keys;

        public object this[string property] => values[property];

        public bool Equals(IDocumentKey other) {
            return Equals((object) other);
        }

        public bool Empty => values == null || values.Count == 0;

        public bool Equals(DocumentKey other) {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (Empty)
                return false;

            return values.SequenceEqual(other.values);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(DocumentKey))
                return false;

            return Equals((DocumentKey) obj);
        }

        public override int GetHashCode() {
            if (Empty)
                return 0;

            unchecked {
                var hash = values.Count;
                values.Apply(kv => hash += kv.Key.GetHashCode() + (kv.Value != null ? kv.Value.GetHashCode() : 0));
                return hash;
            }
        }

        private Query ConvertToQueryExpression(KeyValuePair<string, object> kvp) {
            var mapping = mappings[kvp.Key];

            var term = mapping.ConvertToQueryExpression(kvp.Value);
            if (string.IsNullOrWhiteSpace(term))
                throw new InvalidOperationException("Value for key field '" + kvp.Key + "' cannot be null or empty.");

            return mapping.CreateQuery(term);
        }
    }
}
