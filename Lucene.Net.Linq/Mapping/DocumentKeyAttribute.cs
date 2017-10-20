/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     When set on a class, defines a fixed-value key that will always
    ///     be used when querying for objects of this type or deleting and
    ///     replacing documents with matching keys.
    ///     This attribute enables multiple object types to be stored in
    ///     the same index by ensuring that unrelated documents of other
    ///     types will not be returned when querying.
    /// </summary>
    /// <example>
    ///     <code>
    ///   [DocumentKey(FieldName="Type", Value="Customer")]
    ///   public class Customer
    ///   {
    ///   }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DocumentKeyAttribute : Attribute {
        /// <summary>
        ///     The field name that will be queried.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     The constant value that will be queried.
        /// </summary>
        public string Value { get; set; }
    }
}
