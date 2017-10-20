/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.ComponentModel;
using System.Globalization;

namespace Sprockets.Lucene.Net.Linq.Converters {
    public class DateTimeOffsetToTicksConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(DateTimeOffset);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(long);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            return new DateTimeOffset((long) value, TimeSpan.Zero);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType) {
            return ((DateTimeOffset) value).Ticks;
        }
    }
}
