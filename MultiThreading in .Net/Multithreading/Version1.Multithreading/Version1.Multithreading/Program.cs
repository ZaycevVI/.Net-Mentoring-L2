using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Version1.Multithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            //var task = Task1();
            //var task = Task2();
            //var task = Task3(new[]
            //{
            //    new []{2, 2, 3},
            //    new []{5, 1, 8}
            //},
            //new[]
            //{
            //    new []{2, 7},
            //    new []{3, 9},
            //    new []{4, 6}
            //});
            //Task4();
            Task5();
            //Task6();
            //Task7();


            //task.Wait();
        }

        // Write a program, which creates an array of 100 Tasks, 
        // runs them and wait all of them are not finished. Each Task should iterate from 1 to 1000 and 
        // print into the console the following string: "Task #0 – {iteration number}".
        public static Task Task1()
        {
            var tasks = new List<Task>();
            for (var i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (var j = 1; j <= 1000; j++)
                    {
                        Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}; \t Iteration index: {j}.");
                    }
                }));
            }

            return Task.WhenAll(tasks);
        }

        // Write a program, which creates a chain of four Tasks. First Task – creates an array of 10 random integer.
        // Second Task – multiplies this array with another random integer.Third Task – sorts this array by ascending.
        // Fourth Task – calculates the average value. All this tasks should print the values to console
        public static Task Task2()
        {
            var random = new Random();

            return Task.Run(() =>
                {
                    var result = Enumerable
                        .Repeat(0, 10)
                        .Select(i => random.Next(1, 11))
                        .ToArray();

                    Console.WriteLine("Random 10 integer numbers.");
                    Print(result);

                    return result;
                })
                .ContinueWith(task =>
                {
                    var randomNum = random.Next(1, 11);
                    var result = task.Result.Select(item => item * randomNum);

                    Console.WriteLine($"Random 10 integer numbers multiplied by {randomNum}.");
                    Print(result);

                    return result;
                })
                .ContinueWith(task =>
                {
                    var result = task.Result.OrderBy(item => item);
                    Console.WriteLine("Ordered by ascending.");
                    Print(result);

                    return result;
                })
                .ContinueWith(task => Console.WriteLine($"Average value {task.Result.Average()}."));
        }

        // Write a program, which multiplies two matrices and uses class Parallel.
        public static int[][] Task3(int[][] matrixA, int[][] matrixB)
        {
            var aRows = matrixA.Length; var aCols = matrixA[0].Length;
            var bRows = matrixB.Length; var bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("xxxx");

            var result = MatrixCreate(aRows, bCols);

            Parallel.For(0, aRows, i =>
                {
                    for (var j = 0; j < bCols; ++j) // each col of B
                        for (var k = 0; k < aCols; ++k) // could use k < bRows
                            result[i][j] += matrixA[i][k] * matrixB[k][j];
                }
            );

            return result;
        }

        // Write a program which recursively creates 10 threads. 
        // Each thread should be with the same body and receive a state with integer number, 
        // decrement it, print and pass as a state into the newly created thread. Use Thread class for this task and Join for waiting threads.
        public static void Task4()
        {
            void CreateThread(object arg)
            {
                var c = (int)arg;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started with index: {c}");

                if (c == 0)
                    return;

                var thread = new Thread(CreateThread);
                thread.Start(--c);
                thread.Join();
            }

            CreateThread(10);
        }

        // Write a program which recursively creates 10 threads. Each thread should be with the same body 
        // and receive a state with integer number, decrement it, print and pass as a state into the newly created thread. 
        // Use ThreadPool class for this task and Semaphore for waiting threads.
        public static void Task5()
        {
            var semaphore = new Semaphore(1, 1);

            void CreateThread(object arg)
            {

                var c = (int)arg;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started with index: {c}");

                if (c == 0)
                {
                    semaphore.Release();
                    return;
                }

                ThreadPool.QueueUserWorkItem(CreateThread, --c);
            }

            CreateThread(10);
            semaphore.WaitOne();
        }


        private static readonly object Sync = new object();

        // Write a program which creates two threads and a shared collection: 
        // the first one should add 10 elements into the collection and the second should print 
        // all elements in the collection after each adding. Use Thread, ThreadPool or Task
        // classes for thread creation and any kind of synchronization constructions.
        public static void Task6()
        {
            var list = new List<int>();
            var resetEvent = new AutoResetEvent(false);

            var thread = new Thread(() =>
            {
                Console.WriteLine($"Loop id: {Thread.CurrentThread.ManagedThreadId}");

                for (var i = 0; i < 10; i++)
                {
                    lock (Sync)
                    {
                        list.Add(i);
                    }

                    Task.Run(() =>
                    {
                        Console.WriteLine($"Handler id: {Thread.CurrentThread.ManagedThreadId}");
                        lock (Sync)
                        {
                            Console.WriteLine("==============================");
                            foreach (var item in list)
                            {
                                Console.WriteLine(item);
                            }
                            Console.WriteLine("==============================");
                            resetEvent.Set();
                        }
                    });

                    resetEvent.WaitOne();
                }
            });

            thread.Start();
            thread.Join();
        }

        // Create a Task and attach continuations to it according to the following criteria:
        public static void Task7()
        {
            // a. Continuation task should be executed regardless of the result of the parent task.
            Task.Run(() => throw new NullReferenceException())
                .ContinueWith(task => Console.WriteLine("Regardless of the result")).Wait();

            // b. Continuation task should be executed when the parent task finished without success
            Task.Run(() => throw new NullReferenceException())
                .ContinueWith(task =>
                {
                    Console.WriteLine("Parent task finished without success");
                }, TaskContinuationOptions.OnlyOnFaulted).Wait();

            // d. Continuation task should be executed outside of the thread pool when the parent task would be canceled
            var resetEvent = new AutoResetEvent(false);
            var tokenSource = new CancellationTokenSource();
            var ct = tokenSource.Token;
            tokenSource.Cancel();

            Console.WriteLine($"Main id: {Thread.CurrentThread.ManagedThreadId}");
            var t = Task.FromCanceled(ct).ContinueWith(task =>
            {
                resetEvent.WaitOne();
                Console.WriteLine("Continuation was executed outside chain of tasks");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task.Delay(2000).Wait();
            resetEvent.Set();
            t.Wait();

        }

        public static void Print<T>(IEnumerable<T> collection)
        {
            Console.WriteLine("=============================");

            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("=============================");
        }

        public static int[][] MatrixCreate(int rows, int cols)
        {
            var result = new int[rows][];
            for (var i = 0; i < rows; ++i)
                result[i] = new int[cols];

            return result;
        }
    }
}
