using System;

namespace Task1
{
    class Program
    {
        // Напишите консольное приложение для асинхронного расчета суммы целых чисел от 0 до N. 
        // N задается пользователем из консоли. Пользователь вправе внести новую границу в процессе вычислений, 
        // что должно привести к перезапуску расчета. Это не должно привести к «падению» приложения.
        static void Main(string[] args)
        {
            var isExit = true;

            Console.WriteLine("Enter upper limit: ");
            var upperLimitStr = Console.ReadLine();
            var upperLimit = Convert.ToUInt64(upperLimitStr);
            var calculator = new Calculator(upperLimit);
            var task = calculator.CalculateAsync();

            do
            {
                Console.WriteLine("Enter new upper limit: ");
                upperLimitStr = Console.ReadLine();
                upperLimit = Convert.ToUInt64(upperLimitStr);
                calculator.UpperLimit = upperLimit;

                Console.WriteLine("Do you want to exit? (y/n)");
                var exitCommand = Console.ReadLine();
                isExit = exitCommand == "y";
            } while (!isExit);

            Console.WriteLine($"Result is: {task.Result}.");
        }
    }
}
