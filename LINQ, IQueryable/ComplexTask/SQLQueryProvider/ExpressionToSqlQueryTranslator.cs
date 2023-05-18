using SQLQueryProvider.QueryProvider;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace SQLQueryProvider
{
    public class ExpressionToSqlQueryTranslator : ExpressionVisitor
    {
        private readonly StringBuilder _resultStringBuilder = new StringBuilder();

        public string Translate(Expression expression)
        {
            Visit(expression);
            return _resultStringBuilder.ToString();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append($"{node.Member.Name} ");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(SqlEntitySet<>))
            {
                _resultStringBuilder.Append($"select * from {node.Type.GenericTypeArguments[0].Name} ");
                return node;
            }

            switch (node.Value)
            {
                case int:
                    _resultStringBuilder.Append($"{node.Value} ");
                    break;
                case string:
                    _resultStringBuilder.Append($"'{node.Value}' ");
                    break;
                default:
                    throw new NotSupportedException($"Type '{node.Type}' is not supported");
            }

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    VisitNodes(node.Left, node.Right, "= ");
                    break;
                case ExpressionType.GreaterThan:
                    VisitNodes(node, "> ");
                    break;
                case ExpressionType.LessThan:
                    VisitNodes(node, "< ");
                    break;
                case ExpressionType.AndAlso:
                    VisitNodes(node.Left, node.Right, "and ");
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                _resultStringBuilder.Append($"select * from {node.Type.GenericTypeArguments[0].Name} where ");

                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.Name == "CompareTo")
            {
                Visit(node.Object);
                return node;
            }
            return base.VisitMethodCall(node);
        }


        private void VisitNodes(Expression left, Expression right, string @operator)
        {
            Visit(left);
            _resultStringBuilder.Append(@operator);
            Visit(right);
        }
        private void VisitNodes(BinaryExpression node, string @operator)
        {
            if (node.Left is MethodCallExpression methodNode)
            {
                VisitNodes(methodNode, methodNode.Arguments[0], @operator);
                return;
            }

            VisitNodes(node.Left, node.Right, @operator);
        }
    }
}
