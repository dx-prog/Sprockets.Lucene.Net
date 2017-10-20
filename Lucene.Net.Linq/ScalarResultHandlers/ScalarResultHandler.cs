/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using Lucene.Net.Search;

namespace Sprockets.Lucene.Net.Linq.ScalarResultHandlers {
    internal abstract class ScalarResultHandler {
        public abstract IEnumerable<Type> SupportedTypes { get; }

        public T Execute<T>(LuceneQueryModel luceneQueryModel, TopFieldDocs hits) {
            return (T) Convert.ChangeType(Execute(luceneQueryModel, hits), typeof(T));
        }

        protected abstract object Execute(LuceneQueryModel luceneQueryModel, TopFieldDocs hits);
    }
}
