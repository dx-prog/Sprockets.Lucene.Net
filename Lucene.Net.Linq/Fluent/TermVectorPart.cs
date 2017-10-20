/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Sprockets.Lucene.Net.Linq.Mapping;

namespace Sprockets.Lucene.Net.Linq.Fluent {
    public class TermVectorPart<T> {
        private readonly PropertyMap<T> propertyMap;

        internal TermVectorPart(PropertyMap<T> propertyMap) {
            this.propertyMap = propertyMap;
        }

        public PropertyMap<T> No() {
            propertyMap.TermVectorMode = TermVectorMode.No;
            return propertyMap;
        }

        public PropertyMap<T> Yes() {
            propertyMap.TermVectorMode = TermVectorMode.Yes;
            return propertyMap;
        }

        public PropertyMap<T> Offsets() {
            propertyMap.TermVectorMode = TermVectorMode.WithOffsets;
            return propertyMap;
        }

        public PropertyMap<T> Positions() {
            propertyMap.TermVectorMode = TermVectorMode.WithPositions;
            return propertyMap;
        }

        public PropertyMap<T> PositionsAndOffsets() {
            propertyMap.TermVectorMode = TermVectorMode.WithPositionsAndOffsets;
            return propertyMap;
        }
    }
}
