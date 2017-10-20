/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <summary>
    ///     Customizes how a property is converted to a field as well as
    ///     storage and indexing options.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : BaseFieldAttribute {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public FieldAttribute()
            : this(IndexMode.Analyzed) {
        }

        /// <param name="indexMode">How the field should be indexed for searching and sorting.</param>
        public FieldAttribute(IndexMode indexMode)
            : this(null, indexMode) {
        }

        /// <param name="field">Backing field used to store data in Lucene index.</param>
        public FieldAttribute(string field)
            : this(field, IndexMode.Analyzed) {
        }

        /// <param name="field">Backing field used to store data in Lucene index.</param>
        /// <param name="indexMode">How the field should be indexed for searching and sorting.</param>
        public FieldAttribute(string field, IndexMode indexMode)
            : base(field) {
            IndexMode = indexMode;
        }

        /// <summary>
        ///     How the field should be indexed for searching and sorting.
        /// </summary>
        public IndexMode IndexMode { get; }

        /// <summary>
        ///     Overrides default format pattern to use when converting ValueType
        ///     to string. If both <c cref="Format">Format</c> and
        ///     <c cref="BaseFieldAttribute.Converter">Converter</c> are specified, <c>Converter</c>
        ///     will take precedence and <c>Format</c> will be ignored.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        ///     When <c>true</c>, causes <c cref="QueryParser.LowercaseExpandedTerms" /> to
        ///     be set to false to prevent wildcard queries like <c>Foo*</c> from being
        ///     converted to lowercase.
        /// </summary>
        public bool CaseSensitive { get; set; }

        /// <summary>
        ///     Gets or sets the default parser operator.
        /// </summary>
        /// <value>
        ///     The default parser operator.
        /// </value>
        public QueryParser.Operator DefaultParserOperator { get; set; }

        /// <summary>
        ///     When set, supplies a custom analyzer for this field. The analyzer type
        ///     must have either a parameterless public constructor, or a public constructor
        ///     that accepts a <see cref="Lucene.Net.Util.Version" /> argument.
        ///     When an external Analyzer is provided on <see cref="LuceneDataProvider" />
        ///     methods it will override this setting.
        /// </summary>
        public Type Analyzer { get; set; }

        /// <summary>
        ///     Maps to <see cref="Field.TermVector" />
        /// </summary>
        public TermVectorMode TermVector { get; set; }

        /// <summary>
        ///     When <c>true</c> and the property implements <see cref="IComparable" />
        ///     and/or <see cref="IComparable{T}" />, instructs the mapping engine to
        ///     use <see cref="SortField.STRING" /> instead of converting each field
        ///     and using <see cref="IComparable{T}.CompareTo" />. This is a performance
        ///     enhancement in cases where the string representation of a complex type
        ///     is alphanumerically sortable.
        /// </summary>
        public bool NativeSort { get; set; }
    }
}
