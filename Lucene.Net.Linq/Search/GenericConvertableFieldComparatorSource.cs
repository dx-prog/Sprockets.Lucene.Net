/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.ComponentModel;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Search {
    internal class GenericConvertableFieldComparatorSource : FieldComparatorSource {
        private readonly TypeConverter converter;
        private readonly Type type;

        public GenericConvertableFieldComparatorSource(Type type, TypeConverter converter) {
            this.type = type;
            this.converter = converter;
        }

        public override FieldComparator NewComparator(string fieldname, int numHits, int sortPos, bool reversed) {
            var genericType = typeof(GenericConvertableFieldComparator<>).MakeGenericType(type);
            var ctr = genericType.GetConstructor(new[] {typeof(int), typeof(string), typeof(TypeConverter)});
            return (FieldComparator) ctr.Invoke(new object[] {numHits, fieldname, converter});
        }

        public class GenericConvertableFieldComparator<TComparable> : GenericFieldComparator<TComparable>
            where TComparable : IComparable<TComparable> {
            private readonly TypeConverter converter;

            public GenericConvertableFieldComparator(int numHits, string field, TypeConverter converter)
                : base(numHits, field) {
                this.converter = converter;
            }

            protected override TComparable[] GetCurrentReaderValues(IndexReader reader, int docBase) {
                var strings = FieldCache_Fields.DEFAULT.GetStrings(reader, field);
                return strings.Select(s => s == null ? default(TComparable) : converter.ConvertFrom(s))
                    .Cast<TComparable>()
                    .ToArray();
            }
        }
    }
}
