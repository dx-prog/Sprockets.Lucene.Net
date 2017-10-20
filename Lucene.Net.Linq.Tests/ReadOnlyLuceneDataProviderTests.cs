// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Lucene.Net.Store;
using Lucene.Net.Util;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq;

namespace Lucene.Net.Linq.Tests
{
    [TestFixture]
    public class ReadOnlyLuceneDataProviderTests
    {
        [Test]
        public void IndexWriterIsNull()
        {
            var provider = new ReadOnlyLuceneDataProvider(new RAMDirectory(), Version.LUCENE_30);
            
            Assert.That(provider.IndexWriter, Is.Null, "provider.IndexWriter");
        }

        [Test]
        public void DisposeAvoidsNRE()
        {
            var provider = new ReadOnlyLuceneDataProvider(new RAMDirectory(), Version.LUCENE_30);

            TestDelegate call = provider.Dispose;

            Assert.That(call, Throws.Nothing);
        }

        [Test]
        public void OpenSessionThrows()
        {
            var provider = new ReadOnlyLuceneDataProvider(new RAMDirectory(), Version.LUCENE_30);

            TestDelegate call = () => provider.OpenSession<Record>();

            Assert.That(call, Throws.InvalidOperationException);
        }
    }
}
