/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Reflection;
using Lucene.Net.Documents;
using Lucene.Net.Util;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq.Fluent {
    /// <summary>
    ///     Extends <see cref="PropertyMap{T}" /> to allow a property
    ///     to be indexed as a <see cref="NumericField" /> with a
    ///     given precision step. See <see cref="PropertyMap{T}.AsNumericField" />
    /// </summary>
    public class NumericPropertyMap<T> : PropertyMap<T> {
        private int precisionStep = NumericUtils.PRECISION_STEP_DEFAULT;

        internal NumericPropertyMap(ClassMap<T> classMap, PropertyInfo propInfo, PropertyMap<T> copy) : base(classMap,
            propInfo,
            copy) {
        }

        /// <summary>
        ///     Sets the precision step for the field. Defaults to <see cref="NumericUtils.PRECISION_STEP_DEFAULT" />.
        /// </summary>
        public NumericPropertyMap<T> WithPrecisionStep(int precisionStep) {
            this.precisionStep = precisionStep;
            return this;
        }

        protected internal override ReflectionFieldMapper<T> ToFieldMapperInternal() {
            var attrib = new NumericFieldAttribute(fieldName) {
                Boost = boost,
                ConverterInstance = converter,
                PrecisionStep = precisionStep,
                Store = store
            };

            return NumericFieldMappingInfoBuilder.BuildNumeric<T>(propInfo, PropertyType, attrib);
        }
    }
}
