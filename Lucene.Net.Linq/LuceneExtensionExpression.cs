/* Modified by David Garcia OCT-19-2017
 * Restyle and reformat files, update namespaces to prevent assembly conflicts for other people
 */
using System;
using System.Linq.Expressions;

namespace Sprockets.Lucene.Net.Linq {
    public abstract class LuceneExtensionExpression : Expression {
        public const ExpressionType CustomNodeType = (ExpressionType) 150001;

        protected LuceneExtensionExpression(Type type)
            : this(type, CustomNodeType) {
        }


        protected LuceneExtensionExpression(Type type, ExpressionType nodeType) {
            Type = type;
            NodeType = nodeType;
        }

        public override ExpressionType NodeType { get; }
        public override Type Type { get; }
        public override bool CanReduce => false;
    }
}
