/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            var visitor = new IncDecExpressionVisitor();

            Expression<Func<int, int>> exp = value => value + 1;
            Console.WriteLine(visitor.Visit(exp).ToString());

            exp = value => value - 1;
            Console.WriteLine(visitor.Visit(exp).ToString());

            Expression<Func<string, int, double, string>> expression = (stringValue, intValue, doubleValue) => 
            $"{intValue + 125} - {doubleValue + 22.5} {stringValue}";

            Console.WriteLine(expression);

            var replaced = visitor.Replace(expression, new Dictionary<string, object>
            {
                { "stringValue", "constant string" },
                { "intValue", 5 },
                { "doubleValue", 5.5 }
            });
            Console.WriteLine(replaced);

            Console.ReadLine();
        }
    }
}
