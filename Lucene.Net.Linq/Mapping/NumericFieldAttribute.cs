/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     Maps a <c cref="ValueType" />, or any type that can be converted
    ///     to <c cref="int" />, <c cref="long" />, <c cref="double" />, or
    ///     <c cref="float" /> to a <c cref="NumericField" /> that will be
    ///     indexed as a trie structure to enable more efficient range filtering
    ///     and sorting.
    /// </summary>
    /// <see cref="NumericField" />
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericFieldAttribute : BaseFieldAttribute {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public NumericFieldAttribute()
            : this(null) {
        }

        /// <param name="field">Backing field used to store data in Lucene index.</param>
        public NumericFieldAttribute(string field)
            : base(field) {
            PrecisionStep = NumericUtils.PRECISION_STEP_DEFAULT;
        }

        /// <see cref="NumericRangeQuery" />
        public int PrecisionStep { get; set; }
    }
}
