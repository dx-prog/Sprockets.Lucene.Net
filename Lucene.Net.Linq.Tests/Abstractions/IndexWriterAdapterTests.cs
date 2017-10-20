// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq.Abstractions;

namespace Lucene.Net.Linq.Tests.Abstractions
{
    [TestFixture]
    public class IndexWriterAdapterTests
    {
        [Test]
        public void SetsFlagOnDispose()
        {
            var target = new IndexWriter(new RAMDirectory(), new KeywordAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);

            var adapter = new IndexWriterAdapter(target);

            adapter.Dispose();

            Assert.That(adapter.IsClosed, Is.True, "Should set flag on Dispose");
        }

        [Test]
        public void SetsFlagOnRollback()
        {
            var target = new IndexWriter(new RAMDirectory(), new KeywordAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);
            
            var adapter = new IndexWriterAdapter(target);
            
            adapter.Rollback();
            
            Assert.That(adapter.IsClosed, Is.True, "Should set flag on Dispose");
        }
    }
}

