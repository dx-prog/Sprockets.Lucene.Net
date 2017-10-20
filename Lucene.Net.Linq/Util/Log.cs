/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;

namespace Sprockets.Lucene.Net.Linq.Util {
    [Obsolete("This library now uses Common.Logging to log messages.")]
    public static class Log {
        [Obsolete("This library now uses Common.Logging to log messages.")]
        public static bool TraceEnabled { get; set; }
    }
}
