/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Remotion.Linq.Clauses;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal abstract class ResultOperatorHandler {
        public abstract IEnumerable<Type> SupportedTypes { get; }
        public abstract void Accept(ResultOperatorBase resultOperator, LuceneQueryModel model);
    }

    internal abstract class ResultOperatorHandler<TOperator> : ResultOperatorHandler
        where TOperator : ResultOperatorBase {
        private readonly MethodInfo genericMethod;

        protected ResultOperatorHandler() {
            var methods = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);

            genericMethod = methods.Single(m => m.Name == "AcceptInternal");
        }

        public sealed override IEnumerable<Type> SupportedTypes => new[] {typeof(TOperator)};

        public sealed override void Accept(ResultOperatorBase resultOperator, LuceneQueryModel model) {
            genericMethod.Invoke(this, new object[] {resultOperator, model});
        }

        protected abstract void AcceptInternal(TOperator resultOperator, LuceneQueryModel model);
    }
}
