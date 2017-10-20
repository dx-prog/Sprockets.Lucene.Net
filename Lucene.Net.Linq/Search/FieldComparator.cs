/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Search {
    public abstract class FieldComparator<T> : FieldComparator {
        protected T bottom;
        protected T[] currentReaderValues;
        protected string field;
        protected T[] values;

        protected FieldComparator(int numHits, string field) {
            this.field = field;
            values = new T[numHits];
        }

        public override void Copy(int slot, int doc) {
            values[slot] = currentReaderValues[doc];
        }

        public override void SetBottom(int bottom) {
            this.bottom = values[bottom];
        }

        public override void SetNextReader(IndexReader reader, int docBase) {
            currentReaderValues = GetCurrentReaderValues(reader, docBase);
        }

        protected abstract T[] GetCurrentReaderValues(IndexReader reader, int docBase);
    }
}
