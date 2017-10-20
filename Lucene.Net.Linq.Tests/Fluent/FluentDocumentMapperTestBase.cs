// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using System.Collections.Generic;
using Sprockets.Lucene.Net.Linq.Fluent;
using Sprockets.Lucene.Net.Linq.Mapping;
using NUnit.Framework;
using Version = Lucene.Net.Util.Version;

namespace Lucene.Net.Linq.Tests.Fluent
{
    public class FluentDocumentMapperTestBase
    {
        protected SampleMap map;

        public class Sample
        {
            public string Name { get; set; }

            public int Id { get; set; }

            public float Score { get; set; }

            public DateTimeOffset Date { get; set; }

            public IEnumerable<int> Numbers { get; set; }

            public IEnumerable<Uri> Urls { get; set; } 

            public float Boost { get; set; }
        }

        public class SampleMap : ClassMap<Sample>
        {
            public SampleMap()
                :base(Version.LUCENE_30)
            {
            }
        }

        [SetUp]
        public void SetUp()
        {
            map = new SampleMap();
        }

        protected ReflectionFieldMapper<Sample> GetMappingInfo(string propertyName)
        {
            return GetMappingInfo<ReflectionFieldMapper<Sample>>(propertyName);
        }

        protected TMapper GetMappingInfo<TMapper>(string propertyName)
        {
            return (TMapper)map.ToDocumentMapper().GetMappingInfo(propertyName);
        }
    }
}
