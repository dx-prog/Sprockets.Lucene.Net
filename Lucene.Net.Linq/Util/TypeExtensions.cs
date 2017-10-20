/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Util {
    internal static class TypeExtensions {
        internal static int ToSortField(this Type valueType) {
            if (valueType == typeof(long))
                return SortField.LONG;
            if (valueType == typeof(int))
                return SortField.INT;
            if (valueType == typeof(double))
                return SortField.DOUBLE;
            if (valueType == typeof(float))
                return SortField.FLOAT;

            return SortField.CUSTOM;
        }

        internal static Type GetUnderlyingType(this Type type) {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}
