/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     When set on a property, the document boost will be set with the property
    ///     value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DocumentBoostAttribute : Attribute {
    }
}
