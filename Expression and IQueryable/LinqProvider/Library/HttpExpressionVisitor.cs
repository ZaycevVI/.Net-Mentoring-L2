using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Library;

namespace Package
{
    public class HttpExpressionVisitor : ExpressionVisitor
    {
        private List<QueryDto> _result;
        private QueryDto _currentEntity;

        public List<QueryDto> Translate(Expression exp)
        {
            _result = new List<QueryDto>();

            Visit(exp);

            return _result;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                _currentEntity = new QueryDto();
                _result.Add(_currentEntity);
                _currentEntity.MethodType = ResolveStringMethodType(node.Method.Name);
                Visit(node.Object);
                Visit(node.Arguments[0]);
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression constant = null;
            Expression member = null;

            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (TryGetOperands(node.Left, node.Right, ref member, ref constant))
                        throw new NotSupportedException("One operand should be property and another - constant.");

                    _currentEntity = new QueryDto();
                    _result.Add(_currentEntity);
                    _currentEntity.MethodType = MethodType.Equals;
                    break;
                case ExpressionType.AndAlso:
                    break;
                default:
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
            }

            Visit(node.Left);
            Visit(node.Right);

            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var lambda = node as LambdaExpression;

            Visit(lambda.Body);

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _currentEntity.FieldName = node.Member.Name;

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _currentEntity.Value = node.Value.ToString();

            return node;
        }

        private bool TryGetOperands(Expression left, Expression right, ref Expression member, ref Expression constant)
        {
            if (left.NodeType == ExpressionType.MemberAccess 
                && right.NodeType == ExpressionType.Constant 
                ||
                left.NodeType == ExpressionType.Constant 
                && right.NodeType == ExpressionType.MemberAccess)
                return false;

            member = left.NodeType == ExpressionType.MemberAccess ? left : right;
            constant = left.NodeType == ExpressionType.Constant ? left : right;
            return true;
        }

        private MethodType ResolveStringMethodType(string methodName)
        {
            switch (methodName)
            {
                case "StartsWith":
                    return MethodType.StartsWith;
                case "EndsWith":
                    return MethodType.EndsWith;
                case "Contains":
                    return MethodType.Contains;
                default:
                    throw new NotSupportedException($"Method {methodName} is not supported");
            }
        }
    }
}
