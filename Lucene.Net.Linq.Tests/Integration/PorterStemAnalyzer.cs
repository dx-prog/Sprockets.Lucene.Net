// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Lucene.Net.Linq.Tests.Integration
{
    public class PorterStemAnalyzer : StandardAnalyzer
    {
        public PorterStemAnalyzer(Version version)
            : base(version)
        {
        }

        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            return new PorterStemFilter(base.TokenStream(fieldName, reader));
        }
    }
}
