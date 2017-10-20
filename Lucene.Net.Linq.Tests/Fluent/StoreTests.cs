// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using Sprockets.Lucene.Net.Linq.Fluent;
using Sprockets.Lucene.Net.Linq.Mapping;
using NUnit.Framework;

namespace Lucene.Net.Linq.Tests.Fluent
{
    [TestFixture]
    public class StoreTests : FluentDocumentMapperTestBase
    {
        [Test]
        public void Stored()
        {
            Test(p => p.Stored(), StoreMode.Yes);
        }

        [Test]
        public void NotStored()
        {
            Test(p => p.NotStored(), StoreMode.No);
        }

        protected void Test(Action<PropertyMap<Sample>> setStoreMode, StoreMode expectedStoreMode)
        {
            setStoreMode(map.Property(x => x.Name));
            var mapper = GetMappingInfo("Name");

            Assert.That(mapper.Store, Is.EqualTo(expectedStoreMode));
        }
    }
}
