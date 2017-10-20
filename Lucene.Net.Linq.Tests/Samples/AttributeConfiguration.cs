// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System;
using System.Linq;
using Lucene.Net.Store;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq;
using Version = Lucene.Net.Util.Version;

namespace Lucene.Net.Linq.Tests.Samples
{
    [TestFixture]
    public class AttributeConfiguration
    {
        public static void Main()
        {
            var directory = new RAMDirectory();

            var provider = new LuceneDataProvider(directory, Version.LUCENE_30);

            // add some documents
            using (var session = provider.OpenSession<Article>())
            {
                session.Add(new Article {Author = "John Doe", BodyText = "some body text", PublishDate = DateTimeOffset.UtcNow, Boost = 2f});
            }

            var articles = provider.AsQueryable<Article>();

            var threshold = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(30));

            var articlesByJohn = from a in articles
                                 where a.Author == "John Doe" && a.PublishDate > threshold
                                 orderby a.Title
                                 select a;


            Console.WriteLine("Articles by John Doe: " + articlesByJohn.Count());

            var searchResults = from a in articles
                                where a.SearchText == "some search query"
                                select a;

            Console.WriteLine("Search Results: " + searchResults.Count());
        }

        [Test, Explicit]
        public void RunMain()
        {
            Main();
        }
    }
}
