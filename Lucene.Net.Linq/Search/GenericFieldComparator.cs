/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;

namespace Sprockets.Lucene.Net.Linq.Search {
    public abstract class GenericFieldComparator<T> : FieldComparator<T> where T : IComparable<T> {
        protected GenericFieldComparator(int numHits, string field)
            : base(numHits, field) {
        }

        public override IComparable this[int slot] => new ComparableWrapper(values[slot]);

        public override int Compare(int slot1, int slot2) {
            return values[slot1].CompareTo(values[slot2]);
        }

        public override int CompareBottom(int doc) {
            return bottom.CompareTo(currentReaderValues[doc]);
        }

        private class ComparableWrapper : IComparable {
            private readonly T value;

            public ComparableWrapper(T value) {
                this.value = value;
            }

            public int CompareTo(object obj) {
                var other = (T) obj;
                return value.CompareTo(other);
            }
        }
    }
}
