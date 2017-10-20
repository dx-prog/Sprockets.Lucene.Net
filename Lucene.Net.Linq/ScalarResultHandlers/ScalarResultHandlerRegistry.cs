/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using Sprockets.Lucene.Net.Linq.Util;
using ThirdParty.LGPL.V2._1;

namespace Sprockets.Lucene.Net.Linq.ScalarResultHandlers {
    internal class ScalarResultHandlerRegistry : RegistryBase<ScalarResultHandlerRegistry, Type, ScalarResultHandler> {
        public static ScalarResultHandlerRegistry Instance { get; } = CreateDefault();

        public override ScalarResultHandler GetItem(Type key) {
            return GetItemExact(key);
        }

        protected override void RegisterForTypes(IEnumerable<Type> itemTypes) {
            itemTypes.Apply(RegisterForType);
        }

        private void RegisterForType(Type type) {
            var handler = (ScalarResultHandler) Activator.CreateInstance(type);
            Register(handler.SupportedTypes, handler);
        }
    }
}
