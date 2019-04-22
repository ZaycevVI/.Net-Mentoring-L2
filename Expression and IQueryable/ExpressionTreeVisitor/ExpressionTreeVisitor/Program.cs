using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTreeVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = new Dictionary<string, int>();
            var newValue = 1000;
            var visitor = new ExpressionTreeVisitor(dictionary);
            Expression<Func<int, int>> increment = num => num + 1;
            Expression<Func<int, int>> decrement = num => num - 1;

            var decrementExpression = (LambdaExpression)visitor.Visit(decrement);
            var incrementExpression = (LambdaExpression)visitor.Visit(increment);
            
            Console.WriteLine($"{increment}{Environment.NewLine}{incrementExpression}");
            Console.WriteLine("==============================");
            Console.WriteLine($"{decrement}{Environment.NewLine}{decrementExpression}");
            Console.WriteLine("==============================");
            dictionary["num"] = newValue;
            decrementExpression = (LambdaExpression)visitor.Visit(decrement);
            incrementExpression = (LambdaExpression)visitor.Visit(increment);
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Replace num variable to {newValue}");
            Console.WriteLine($"{increment}{Environment.NewLine}{incrementExpression}");
            Console.WriteLine("==============================");
            Console.WriteLine($"{decrement}{Environment.NewLine}{decrementExpression}");
            Console.WriteLine("==============================");
        }
    }

    public class ExpressionTreeVisitor : ExpressionVisitor
    {
        private Dictionary<string, int> _parameterToValue;

        public ExpressionTreeVisitor(Dictionary<string, int> parameterToValue)
        {
            _parameterToValue = parameterToValue;
        }

        // Замену выражений вида <переменная> + 1 / <переменная> - 1 на операции инкремента и декремента
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var type = node.NodeType;


            if (TryGetVariable(node.Left, node.Right, out Expression variable))
            {
                switch (type)
                {
                    case ExpressionType.Add:
                        return Expression.Increment(variable);
                    case ExpressionType.Subtract:
                        return Expression.Decrement(variable);
                }

                return base.VisitBinary(node);
            }

            else
            {
                return base.VisitBinary(node);
            }
        }

        private bool TryGetVariable(Expression left, Expression right, out Expression variable)
        {
            if (IsConstant(right) && IsVaraible(left))
            {
                var constantExpression = (ConstantExpression)right;
                var variableExpression = (ParameterExpression)left;
                var constantValue = constantExpression.Value;

                if (constantValue is int intValue)
                {
                    if (intValue == 1 && variableExpression.Type == typeof(int))
                    {
                        variable = Replace(variableExpression);
                        return true;
                    }
                }
            }

            variable = null;
            return false;
        }

        // Замену параметров, входящих в lambda-выражение, на константы (в качестве параметров такого преобразования передавать:
        // - Исходное выражение
        // - Список пар<имя параметра: значение для замены>
        private Expression Replace(ParameterExpression expression)
        {
            if (_parameterToValue.TryGetValue(expression.Name, out var value))
            {
                return Expression.Constant(value);
            }

            return expression;
        }
        

        private bool IsConstant(Expression expression) => expression.NodeType == ExpressionType.Constant;

        private bool IsVaraible(Expression expression) => expression.NodeType == ExpressionType.Parameter;
    }
}
