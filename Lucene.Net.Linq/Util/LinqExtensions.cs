/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;

namespace Sprockets.Lucene.Net.Linq.Util {
    public static class LinqExtensions {
        public static void Apply<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source)
                action(item);
        }
    }
}
