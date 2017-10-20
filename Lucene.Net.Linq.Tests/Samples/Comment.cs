// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Lucene.Net.Linq.Tests.Samples {
    public partial class Comment
    {
        [Field(Key = true)]
        public long CommentId { get; set; }
    }
}
