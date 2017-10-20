// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using Lucene.Net.Linq.Tests.Integration;
using Sprockets.Lucene.Net.Linq.Fluent;
using Version = Lucene.Net.Util.Version;

namespace Lucene.Net.Linq.Tests.Samples {
    public class FluentConfiguration {
        public void CreateMapping() {
            var map = new ClassMap<Package>(Version.LUCENE_30);

            map.Key(p => p.Id);
            map.Key(p => p.Version).ConvertWith(new VersionConverter());

            map.Property(p => p.Description)
                .AnalyzeWith(new PorterStemAnalyzer(Version.LUCENE_30))
                .WithTermVector.PositionsAndOffsets();

            map.Property(p => p.DownloadCount)
                .AsNumericField()
                .WithPrecisionStep(8);

            map.Property(p => p.IconUrl).NotIndexed();

            map.Score(p => p.Score);

            map.DocumentBoost(p => p.Boost);
        }

        public class Package {
            public string Id { get; set; }

            public Version Version { get; set; }

            public Uri IconUrl { get; set; }

            public string Description { get; set; }

            public int DownloadCount { get; set; }

            public float Score { get; set; }

            public float Boost { get; set; }
        }
    }
}
