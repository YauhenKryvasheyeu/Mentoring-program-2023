using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private Dictionary<string, object> _parameters;

        public Expression Replace(Expression source, Dictionary<string, object> list)
        {
            _parameters = list;
            return Visit(source);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Right is ConstantExpression constant && constant.Type == typeof(int) && (int)constant.Value == 1 )
            {
                if (node.NodeType == ExpressionType.Add)
                {
                    return Expression.Increment(node.Left);
                }

                if (node.NodeType == ExpressionType.Subtract)
                {
                    return Expression.Decrement(node.Left);
                }

                return base.VisitBinary(node);
            }

            return base.VisitBinary(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameters != null && _parameters.ContainsKey(node.Name))
            {
                return Expression.Parameter(node.Type, _parameters[node.Name].ToString());
            }

            return base.VisitParameter(node);
        }
    }
}
