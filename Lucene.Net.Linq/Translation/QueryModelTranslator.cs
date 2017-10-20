/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */

using System.Linq;
using Lucene.Net.Search;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Sprockets.Lucene.Net.Linq.Clauses;
using Sprockets.Lucene.Net.Linq.Mapping;
using Sprockets.Lucene.Net.Linq.Translation.ResultOperatorHandlers;
using Sprockets.Lucene.Net.Linq.Translation.TreeVisitors;

namespace Sprockets.Lucene.Net.Linq.Translation {
    internal class QueryModelTranslator : QueryModelVisitorBase, ILuceneQueryModelVisitor {
        private static readonly ResultOperatorRegistry resultOperators = ResultOperatorRegistry.CreateDefault();
        private readonly Context context;

        private readonly IFieldMappingInfoProvider fieldMappingInfoProvider;

        internal QueryModelTranslator(IFieldMappingInfoProvider fieldMappingInfoProvider, Context context) {
            this.fieldMappingInfoProvider = fieldMappingInfoProvider;
            this.context = context;
            Model = new LuceneQueryModel(fieldMappingInfoProvider);
        }

        public LuceneQueryModel Model { get; }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index) {
            var visitor = new QueryBuildingExpressionTreeVisitor(fieldMappingInfoProvider);
            visitor.Visit(whereClause.Predicate);

            Model.AddQuery(visitor.Query);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel) {
            Model.SelectClause = selectClause.Selector;
            Model.OutputDataInfo = selectClause.GetOutputDataInfo();
            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index) {
            var handler = resultOperators.GetItem(resultOperator.GetType());

            if (handler != null)
                handler.Accept(resultOperator, Model);
            else
                Model.ApplyUnsupported(resultOperator);

            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index) {
            foreach (var ordering in orderByClause.Orderings)
                Model.AddSort(ordering.Expression, ordering.OrderingDirection);
        }

        public void VisitBoostClause(BoostClause clause, QueryModel queryModel, int index) {
            Model.AddBoostFunction(clause.BoostFunction);
        }

        public void VisitTrackRetrievedDocumentsClause(TrackRetrievedDocumentsClause clause,
            QueryModel queryModel,
            int index) {
            Model.DocumentTracker = clause.Tracker.Value;
        }

        public void VisitQueryStatisticsCallbackClause(QueryStatisticsCallbackClause clause,
            QueryModel queryModel,
            int index) {
            Model.AddQueryStatisticsCallback(clause.Callback);
        }

        public void Build(QueryModel queryModel) {
            queryModel.Accept(this);

            if (context.Settings.EnableMultipleEntities)
                CreateQueryFilterForKeyFields();
        }

        private void CreateQueryFilterForKeyFields() {
            var filterQuery = fieldMappingInfoProvider.KeyProperties.Aggregate(
                new BooleanQuery(),
                (query, property) => {
                    var fieldMappingInfo = fieldMappingInfoProvider.GetMappingInfo(property);

                    query.Add(fieldMappingInfo.CreateQuery("*"), Occur.MUST);
                    return query;
                });

            if (filterQuery.Clauses.Any())
                Model.Filter = new QueryWrapperFilter(filterQuery);
        }
    }
}
