// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Linq.Tests.Integration;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Lucene.Net.Linq.Tests.Samples {
    public partial class Article
    {
        [Field(Analyzer = typeof(StandardAnalyzer))]
        public string Author { get; set; }

        [Field(Analyzer = typeof(StandardAnalyzer))]
        public string Title { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        // Stores the field as a NumericField
        [NumericField]
        public long Id { get; set; }

        // Stores the field as text
        public int IssueNumber { get; set; }

        [Field(IndexMode.NotIndexed, Store = StoreMode.Yes)]
        public string BodyText { get; set; }

        // Maps to field "text"
        [Field("text", Store = StoreMode.No, Analyzer = typeof(PorterStemAnalyzer))]
        public string SearchText
        {
            get { return string.Join(" ", new[] {Author, Title, BodyText}); }
        }

        // Stores complex type as string with a given TypeConverter
        [Field(Converter = typeof (VersionConverter))]
        public System.Version Version { get; set; }

        // Add IgnoreFieldAttribute to properties that should not be mapped to/from Document
        [IgnoreField]
        public string IgnoreMe { get; set; }

        [DocumentBoost]
        public float Boost { get; set; }
    }
}
