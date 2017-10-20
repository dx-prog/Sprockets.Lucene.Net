// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using System.Collections.Generic;
using System.Reflection;
using Lucene.Net.Documents;
using Sprockets.Lucene.Net.Linq.Mapping;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq;

namespace Lucene.Net.Linq.Tests.Mapping
{
    [TestFixture]
    public class FieldMappingInfoBuilderCollectionComplexTypeTests
    {
        private PropertyInfo info;
        private Document document;

        [Field(Converter = typeof(VersionConverter))]
        public IEnumerable<Version> Versions { get; set; }

        [SetUp]
        public void SetUp()
        {
            info = GetType().GetProperty("Versions");
            document = new Document();
        }

        [Test]
        public void IEnumerableOfString_Build()
        {
            var mapper = CreateMapper();
            Assert.That(mapper.FieldName, Is.EqualTo(info.Name));
        }
        
        [Test]
        public void IEnumerableOfString_CopyToDocument_Values()
        {
            Versions = new[] { new Version(1, 0), new Version(2, 3) };
            CreateMapper().CopyToDocument(this, document);
            Assert.That(document.GetValues("Versions"), Is.EqualTo(new[] { "1.0", "2.3" }));
        }

        [Test]
        public void IEnumerableOfString_CopyFromDocument()
        {
            Versions = new[] { new Version(1, 0) };

            document.Add(new Field("Versions", "5.6", Field.Store.YES, Field.Index.NO));
            document.Add(new Field("Versions", "5.7", Field.Store.YES, Field.Index.NO));

            CreateMapper().CopyFromDocument(document, new QueryExecutionContext(), this);

            Assert.That(Versions, Is.EqualTo(new[] {new Version(5, 6), new Version(5, 7)}));
        }

        private IFieldMapper<FieldMappingInfoBuilderCollectionComplexTypeTests> CreateMapper()
        {
            return FieldMappingInfoBuilder.Build<FieldMappingInfoBuilderCollectionComplexTypeTests>(info);
        }
        
    }
}
