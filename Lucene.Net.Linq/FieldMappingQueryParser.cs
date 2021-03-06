/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System;
using System.Collections.Generic;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sprockets.Lucene.Net.Linq.Mapping;
using Sprockets.Lucene.Net.Linq.Search;
using Version = Lucene.Net.Util.Version;

namespace Sprockets.Lucene.Net.Linq {
    public class FieldMappingQueryParser<T> : QueryParser {
        private static readonly string DefaultField = typeof(FieldMappingQueryParser<T>).FullName + ".DEFAULT_FIELD";
        private readonly string _initialDefaultField;

        [Obsolete("Use constructor with default search field")]
        public FieldMappingQueryParser(Version matchVersion, IDocumentMapper<T> mapper)
            : base(matchVersion, DefaultField, mapper.Analyzer) {
            _initialDefaultField = DefaultField;
            MatchVersion = matchVersion;
            DocumentMapper = mapper;
        }

        public FieldMappingQueryParser(Version matchVersion, string defaultSearchField, IDocumentMapper<T> mapper)
            : base(matchVersion, defaultSearchField, mapper.Analyzer) {
            _initialDefaultField = defaultSearchField;
#pragma warning disable 618
            DefaultSearchProperty = defaultSearchField;
#pragma warning restore 618
            MatchVersion = matchVersion;
            DocumentMapper = mapper;
        }

        /// <summary>
        ///     Sets the default property for queries that don't specify which field to search.
        ///     For an example query like <c>Lucene OR NuGet</c>, if this property is set to <c>SearchText</c>,
        ///     it will produce a query like <c>SearchText:Lucene OR SearchText:NuGet</c>.
        /// </summary>
        [Obsolete("Set the default search field in the constructor instead")]
        public string DefaultSearchProperty { get; set; }

        public Version MatchVersion { get; }

        public IDocumentMapper<T> DocumentMapper { get; }

        public override string Field => DefaultSearchProperty;

        protected override Query GetFieldQuery(string field, string queryText) {
            var mapping = GetMapping(field);

            try {
                var codedQueryText = mapping.ConvertToQueryExpression(queryText);
                return mapping.CreateQuery(codedQueryText);
            }
            catch (Exception ex) {
                throw new ParseException(ex.Message, ex);
            }
        }

        protected override Query GetRangeQuery(string field, string part1, string part2, bool inclusive) {
            var rangeType = inclusive ? RangeType.Inclusive : RangeType.Exclusive;
            var mapping = GetMapping(field);
            try {
                return mapping.CreateRangeQuery(part1, part2, rangeType, rangeType);
            }
            catch (Exception ex) {
                throw new ParseException(ex.Message, ex);
            }
        }

        protected override Query GetFieldQuery(string field, string queryText, int slop) {
            return base.GetFieldQuery(OverrideField(field), queryText, slop);
        }

        protected override Query GetWildcardQuery(string field, string termStr) {
            return base.GetWildcardQuery(OverrideField(field), termStr);
        }

        protected override Query GetPrefixQuery(string field, string termStr) {
            return base.GetPrefixQuery(OverrideField(field), termStr);
        }

        protected override Query GetFuzzyQuery(string field, string termStr, float minSimilarity) {
            return base.GetFuzzyQuery(OverrideField(field), termStr, minSimilarity);
        }

        protected virtual IFieldMappingInfo GetMapping(string field) {
            field = OverrideField(field);

            try {
                return DocumentMapper.GetMappingInfo(field);
            }
            catch (KeyNotFoundException) {
                throw new ParseException("Unrecognized field: '" + field + "'");
            }
        }

        private string OverrideField(string field) {
            if (field == _initialDefaultField)
                field = DefaultSearchProperty;

            return field;
        }
    }
}
