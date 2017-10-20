// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System.Linq.Expressions;
using NUnit.Framework;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing;
using Rhino.Mocks;
using Sprockets.Lucene.Net.Linq;
using Sprockets.Lucene.Net.Linq.Transformation;

namespace Lucene.Net.Linq.Tests.Transformation {
    [TestFixture]
    public class QueryModelTransformerTests {
        [SetUp]
        public void SetUp() {
            mocks = new MockRepository();

            visitor1 = mocks.StrictMock<RelinqExpressionVisitor>();
            visitor2 = mocks.StrictMock<RelinqExpressionVisitor>();
            var visitors = new[] {visitor1, visitor2};
            transformer = new QueryModelTransformer(visitors, visitors);

            using (mocks.Ordered()) {
                visitor1.Expect(v => v.VisitExpression(whereClause.Predicate)).Return(whereClause.Predicate);
                visitor2.Expect(v => v.VisitExpression(whereClause.Predicate)).Return(whereClause.Predicate);
            }

            mocks.ReplayAll();
        }

        private static readonly ConstantExpression constantExpression = Expression.Constant(true);
        private static readonly WhereClause whereClause = new WhereClause(constantExpression);
        private RelinqExpressionVisitor visitor1;
        private RelinqExpressionVisitor visitor2;
        private QueryModelTransformer transformer;

        private readonly QueryModel queryModel =
            new QueryModel(new MainFromClause("i", typeof(Record), Expression.Constant("r")),
                new SelectClause(Expression.Constant("a")));

        private MockRepository mocks;

        private void Verify() {
            mocks.VerifyAll();
        }

        [Test]
        public void VisitsOrderByClause() {
            var orderByClause = new OrderByClause();
            orderByClause.Orderings.Add(new Ordering(constantExpression, OrderingDirection.Asc));

            transformer.VisitOrderByClause(orderByClause, queryModel, 0);

            Verify();
        }

        [Test]
        public void VisitsWhereClause() {
            transformer.VisitWhereClause(whereClause, queryModel, 0);

            Verify();
        }
    }
}
