/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     Specifies that a public property should be ignored by the Lucene.Net.Linq
    ///     mapping engine when converting objects to Documents and vice-versa.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreFieldAttribute : Attribute {
    }
}
