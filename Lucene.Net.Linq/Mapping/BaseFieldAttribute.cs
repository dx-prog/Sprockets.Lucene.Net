/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using System.ComponentModel;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     Base attribute for customizing how properties are stored and indexed.
    /// </summary>
    public abstract class BaseFieldAttribute : Attribute {
        protected BaseFieldAttribute()
            : this(null) {
        }

        protected BaseFieldAttribute(string field) {
            Field = field;
            Store = StoreMode.Yes;
            Boost = 1.0f;
        }

        /// <summary>
        ///     Specifies the name of the backing field that the property value will be mapped to.
        ///     When not specified, defaults to the name of the property being decorated by this attribute.
        /// </summary>
        public string Field { get; }

        /// <summary>
        ///     Set to true to store value in index for later retrieval, or
        ///     false if the field should only be indexed.
        /// </summary>
        public StoreMode Store { get; set; }

        /// <summary>
        ///     Provides a custom TypeConverter implementation that can convert the property type
        ///     to and from strings so they can be stored and indexed by Lucene.Net.
        /// </summary>
        public Type Converter {
            get => ConverterInstance != null ? ConverterInstance.GetType() : null;
            set => ConverterInstance = (TypeConverter) Activator.CreateInstance(value);
        }

        /// <summary>
        ///     Specifies that the property value, combined with any other properties that also
        ///     specify <code>Key = true</code>, represents a unique primary key to the document.
        ///     Key fields are used to replace or delete documents.
        /// </summary>
        public bool Key { get; set; }

        /// <summary>
        ///     Specifies a boost to apply when a document is being analyzed during indexing.
        ///     Defaults to <c>1.0f</c>.
        /// </summary>
        public float Boost { get; set; }

        internal TypeConverter ConverterInstance { get; set; }
    }
}
