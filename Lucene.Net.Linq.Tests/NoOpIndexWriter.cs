// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Abstractions;

namespace Lucene.Net.Linq.Tests {
    public class NoOpIndexWriter : IIndexWriter
    {
        public void Dispose()
        {
        }

        public void AddDocument(Document doc)
        {
        }

        public void DeleteDocuments(Query[] queries)
        {
        }

        public void DeleteAll()
        {
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public void Optimize()
        {
        }

        public IndexReader GetReader()
        {
            return null;
        }

        public bool IsClosed
        {
            get
            {
                return false;
            }
        }
    }
}
