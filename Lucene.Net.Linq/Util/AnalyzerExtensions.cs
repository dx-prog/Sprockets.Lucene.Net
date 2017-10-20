/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

namespace Sprockets.Lucene.Net.Linq.Util {
    internal static class AnalyzerExtensions {
        internal static string Analyze(this Analyzer analyzer, string fieldName, string pattern) {
            return analyzer.GetTerms(fieldName, pattern).Single();
        }

        internal static IEnumerable<string> GetTerms(this Analyzer analyzer, string fieldName, string pattern) {
            TokenStream s;

            try {
                s = analyzer.ReusableTokenStream(fieldName, new StringReader(pattern));
            }
            catch (IOException) {
                s = analyzer.TokenStream(fieldName, new StringReader(pattern));
            }

            try {
                while (s.IncrementToken()) {
                    if (!s.HasAttribute<ITermAttribute>())
                        continue;

                    var attr = s.GetAttribute<ITermAttribute>();
                    yield return attr.Term;
                }
            }
            finally {
                s.Dispose();
            }
        }
    }
}
