// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Sprockets.Lucene.Net.Linq.Mapping;
using NUnit.Framework;

namespace Lucene.Net.Linq.Tests.Fluent
{
    [TestFixture]
    public class ScoreTests : FluentDocumentMapperTestBase
    {
        [Test]
        public void CaptureScore()
        {
            map.Score(x => x.Score);

            var mapper = GetMappingInfo<ReflectionScoreMapper<Sample>>("Score");

            Assert.That(mapper, Is.Not.Null);
        }
    }
}
