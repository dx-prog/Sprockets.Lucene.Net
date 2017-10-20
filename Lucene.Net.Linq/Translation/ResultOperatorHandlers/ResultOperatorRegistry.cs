/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using Sprockets.Lucene.Net.Linq.Util;
using ThirdParty.LGPL.V2._1;

namespace Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers {
    internal class ResultOperatorRegistry : RegistryBase<ResultOperatorRegistry, Type, ResultOperatorHandler> {
        public override ResultOperatorHandler GetItem(Type key) {
            return GetItemExact(key);
        }

        protected override void RegisterForTypes(IEnumerable<Type> itemTypes) {
            itemTypes.Apply(RegisterForType);
        }

        private void RegisterForType(Type type) {
            var handler = (ResultOperatorHandler) Activator.CreateInstance(type);
            Register(handler.SupportedTypes, handler);
        }
    }
}
