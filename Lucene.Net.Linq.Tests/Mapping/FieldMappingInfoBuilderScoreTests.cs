// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System.Collections.Generic;
using System.Reflection;
using Lucene.Net.Documents;
using Sprockets.Lucene.Net.Linq.Mapping;
using Lucene.Net.Search;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq;

namespace Lucene.Net.Linq.Tests.Mapping
{
    [TestFixture]
    public class FieldMappingInfoBuilderScoreTests
    {
        private PropertyInfo info;
        private Document document;

        [QueryScore]
        public float Score { get; set; }

        [SetUp]
        public void SetUp()
        {
            info = GetType().GetProperty("Score");
            document = new Document();
        }

        [Test]
        public void SetsScore()
        {
            const float sampleScore = 0.48f;

            var mapper = CreateMapper();
            mapper.CopyFromDocument(document, new QueryExecutionContext { CurrentScoreDoc = new ScoreDoc(1, sampleScore), Phase = QueryExecutionPhase.ConvertResults }, this);

            Assert.That(Score, Is.EqualTo(sampleScore));
        }

        private IFieldMapper<FieldMappingInfoBuilderScoreTests> CreateMapper()
        {
            return FieldMappingInfoBuilder.Build<FieldMappingInfoBuilderScoreTests>(info);
        }
    }
}
