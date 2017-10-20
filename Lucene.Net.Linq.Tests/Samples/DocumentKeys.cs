// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Lucene.Net.Linq.Tests.Samples {
  
    ///// <remarks>
    ///// In this example a DocumentKey is added to each class
    ///// to add a fixed-value field to each document that will
    ///// be used to distinguish entities of different types.
    ///// 
    ///// In this example both Article and Commend use Id as
    ///// the unique identifier for each entity.
    ///// </remarks> 
    //[DocumentKey(FieldName = "Type", Value = "Article")]
    //public partial class Article
    //{
    //    [Field(Key = true)]
    //    public long Id { get; set; }
    //}

    [DocumentKey(FieldName = "Type", Value = "Comment")]
    public partial class Comment
    {
        [Field(Key = true)]
        public long Id { get; set; }
    }
}
