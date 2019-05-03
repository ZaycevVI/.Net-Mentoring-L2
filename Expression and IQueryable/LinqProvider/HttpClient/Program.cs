using System;
using System.Linq;
using Library;
using Package;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = new HttpQuery<Entity>();

            // query sends request to server (http protocol) and retrieves data from it
            // see HttpLinqProvider
            var query1 = query.Where(e => e.FirstName.Contains("a"));

            Console.WriteLine("e.FirstName.Contains(\"a\"):");
            Print(query1);
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}");
            var query2 = query.Where(e => e.LastName.StartsWith("L") && e.FirstName.Contains("sh"));
            Console.WriteLine("e.LastName.StartsWith(\"L\") && e.LastName.FirstName(\"sh\"):");
            Print(query2);
        }

        static void Print(IQueryable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Console.WriteLine("==============================");
                Console.WriteLine($"\tFirstName: {entity.FirstName}");
                Console.WriteLine($"\tLastName: {entity.LastName}");
                Console.WriteLine("==============================");
            }
        }
    }
}
