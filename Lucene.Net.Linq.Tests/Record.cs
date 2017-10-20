// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Lucene.Net.Linq.Tests
{
    public class Record
    {
        public string Name { get; set; }

        [Field(Key = true)]
        public string Id { get; set; }
    }
}
