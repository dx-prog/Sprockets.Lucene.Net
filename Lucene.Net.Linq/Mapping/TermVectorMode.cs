/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using Lucene.Net.Documents;

namespace Sprockets.Lucene.Net.Linq.Mapping {
    /// <see cref="Field.TermVector" />
    public enum TermVectorMode {
        No = Field.TermVector.NO,
        Yes = Field.TermVector.YES,
        WithOffsets = Field.TermVector.WITH_OFFSETS,
        WithPositions = Field.TermVector.WITH_POSITIONS,
        WithPositionsAndOffsets = Field.TermVector.WITH_POSITIONS_OFFSETS
    }
}
