/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

namespace Sprockets.Lucene.Net.Linq.Fluent {
    /// <summary>
    ///     Defines a document key. See <see cref="ClassMap{T}.DocumentKey" />.
    /// </summary>
    public class DocumentKeyPart<T> {
        private readonly ClassMap<T> classMap;
        private readonly string fieldName;

        internal DocumentKeyPart(ClassMap<T> classMap, string fieldName) {
            this.classMap = classMap;
            this.fieldName = fieldName;
        }

        /// <summary>
        ///     Specify the fixed value for the key. May not be null.
        /// </summary>
        public void WithFixedValue(string value) {
            classMap.SetDocumentKeyValue(fieldName, value);
        }
    }
}
