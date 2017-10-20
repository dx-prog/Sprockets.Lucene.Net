// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprockets.Lucene.Net.Linq.Mapping;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Lucene.Net.Linq.Tests.Fluent
{
    [TestFixture]
    public class DocumentBoostTests : FluentDocumentMapperTestBase
    {
        [Test]
        public void CaptureDocumentBoost()
        {
            map.DocumentBoost(x => x.Boost);

            var mapper = GetMappingInfo<ReflectionDocumentBoostMapper<Sample>>("Boost");

            Assert.That(mapper, Is.Not.Null);
        }
    }
}
