/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using System.Collections.Generic;
using Lucene.Net.Search;
using Remotion.Linq.Clauses.ResultOperators;

namespace Sprockets.Lucene.Net.Linq.ScalarResultHandlers {
    internal class AnyResultHandler : ScalarResultHandler {
        public override IEnumerable<Type> SupportedTypes => new[] {typeof(AnyResultOperator)};

        protected override object Execute(LuceneQueryModel luceneQueryModel, TopFieldDocs hits) {
            return hits.TotalHits - luceneQueryModel.SkipResults > 0;
        }
    }
}
