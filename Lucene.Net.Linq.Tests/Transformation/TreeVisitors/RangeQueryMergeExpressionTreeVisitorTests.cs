// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System.Linq.Expressions;
using Sprockets.Lucene.Net.Linq.Clauses.Expressions;
using Sprockets.Lucene.Net.Linq.Search;
using Sprockets.Lucene.Net.Linq.Transformation.TreeVisitors;
using Lucene.Net.Search;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq;

namespace Lucene.Net.Linq.Tests.Transformation.TreeVisitors
{
    [TestFixture]
    public class RangeQueryMergeExpressionTreeVisitorTests
    {
        private RangeQueryMergeExpressionTreeVisitor visitor;

        private LuceneQueryPredicateExpression lower = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof (int), "percentile"),
            Expression.Constant(70), Occur.MUST, QueryType.GreaterThanOrEqual);

        private LuceneQueryPredicateExpression upper = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof(int), "percentile"),
            Expression.Constant(80), Occur.MUST, QueryType.LessThan);

        [SetUp]
        public void SetUp()
        {
            visitor = new RangeQueryMergeExpressionTreeVisitor();
        }

        [Test]
        public void Merge()
        {
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, lower, upper);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.InstanceOf<LuceneRangeQueryExpression>());
        }

        [Test]
        public void MergeReverseOrder()
        {
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, upper, lower);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.InstanceOf<LuceneRangeQueryExpression>());
            var expr = (LuceneRangeQueryExpression) result;
            Assert.That(expr.Lower, Is.SameAs(lower.QueryPattern));
            Assert.That(expr.Upper, Is.SameAs(upper.QueryPattern));
        }

        [Test]
        public void MergeExclusionaryRange()
        {
            var lowerLess = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof (int), "percentile"),
                Expression.Constant(40), Occur.MUST, QueryType.LessThan);
            var upperGreater = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof(int), "percentile"),
                Expression.Constant(60), Occur.MUST, QueryType.GreaterThan);

            var binary = Expression.MakeBinary(ExpressionType.OrElse, lowerLess, upperGreater);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.InstanceOf<LuceneRangeQueryExpression>());
            var expr = (LuceneRangeQueryExpression)result;
            Assert.That(expr.Lower, Is.SameAs(lowerLess.QueryPattern));
            Assert.That(expr.LowerQueryType, Is.EqualTo(QueryType.GreaterThanOrEqual));
            Assert.That(expr.Upper, Is.SameAs(upperGreater.QueryPattern));
            Assert.That(expr.UpperQueryType, Is.EqualTo(QueryType.LessThanOrEqual));
            Assert.That(expr.Occur, Is.EqualTo(Occur.MUST_NOT));
        }

        [Test]
        public void MergeExclusionaryRangeReverseOrder()
        {
            var lowerLess = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof(int), "percentile"),
                Expression.Constant(40), Occur.MUST, QueryType.LessThanOrEqual);
            var upperGreater = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof(int), "percentile"),
                Expression.Constant(60), Occur.MUST, QueryType.GreaterThanOrEqual);

            var binary = Expression.MakeBinary(ExpressionType.OrElse, upperGreater, lowerLess);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.InstanceOf<LuceneRangeQueryExpression>());
            var expr = (LuceneRangeQueryExpression)result;
            Assert.That(expr.Lower, Is.SameAs(lowerLess.QueryPattern));
            Assert.That(expr.LowerQueryType, Is.EqualTo(QueryType.GreaterThan));
            Assert.That(expr.Upper, Is.SameAs(upperGreater.QueryPattern));
            Assert.That(expr.UpperQueryType, Is.EqualTo(QueryType.LessThan));
            Assert.That(expr.Occur, Is.EqualTo(Occur.MUST_NOT));
        }

        [Test]
        public void IgnoreBothLower()
        {
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, lower, lower);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.SameAs(binary));
        }

        [Test]
        public void IgnoreBothUpper()
        {
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, upper, upper);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.SameAs(binary));
        }

        [Test]
        public void IgnoreNonQueryExpression()
        {
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, lower, Expression.Constant(true));

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.SameAs(binary));
        }

        [Test]
        public void IgnoreNonRangeQueryExpression()
        {
            var equal = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof(int), "percentile"), Expression.Constant(50), Occur.MUST, QueryType.Default);
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, lower, equal);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.SameAs(binary));
        }

        [Test]
        public void IgnoreDifferentFields()
        {
            var otherUpper = new LuceneQueryPredicateExpression(new LuceneQueryFieldExpression(typeof(int), "id"), Expression.Constant(80), Occur.MUST, QueryType.LessThanOrEqual);
            var binary = Expression.MakeBinary(ExpressionType.AndAlso, lower, otherUpper);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.SameAs(binary));
        }

        [Test]
        public void IgnoreNonAndAlso()
        {
            var binary = Expression.MakeBinary(ExpressionType.OrElse, lower, upper);

            var result = visitor.VisitExpression(binary);

            Assert.That(result, Is.SameAs(binary));
        }
    }
}
