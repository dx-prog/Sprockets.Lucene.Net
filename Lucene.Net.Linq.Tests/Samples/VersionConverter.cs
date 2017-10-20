// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using System.ComponentModel;
using System.Globalization;

namespace Lucene.Net.Linq.Tests.Samples {
    public class VersionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (string.IsNullOrWhiteSpace((string)value)) return null;

            return new System.Version((string)value);
        }
    }
}
