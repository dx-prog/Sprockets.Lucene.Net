// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using Sprockets.Lucene.Net.Linq.Converters;
using NUnit.Framework;

namespace Lucene.Net.Linq.Tests.Fluent
{
    [TestFixture]
    public class ConverterTests : FluentDocumentMapperTestBase
    {
        [Test]
        public void SpecifyConverter()
        {
            var converter = new DateTimeOffsetToTicksConverter();

            map.Property(x => x.Date).ConvertWith(converter);

            var mapper = GetMappingInfo("Date");
            
            Assert.That(mapper.Converter, Is.SameAs(converter));
        }

        [Test]
        public void AssignsDefaultConverter()
        {
            map.Property(x => x.Urls);

            var mapper = GetMappingInfo("Urls");

            Assert.That(mapper.Converter, Is.InstanceOf<UriTypeConverter>());
        }
    }
}
