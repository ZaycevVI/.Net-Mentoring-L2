using System.Collections.Generic;
using Library;

namespace HttpServer.Data
{
    public class DataContext
    {
        public List<Entity> Entities => new List<Entity>
        {
            new Entity
            {
                FirstName = "Vova",
                LastName = "Zvyagencev"
            },
            new Entity
            {
                FirstName = "Alex",
                LastName = "Krkic"
            },
            new Entity
            {
                FirstName = "Pasha",
                LastName = "Lisovski"
            },
            new Entity
            {
                FirstName = "Dasha",
                LastName = "Kirkor"
            },
            new Entity
            {
                FirstName = "Nastia",
                LastName = "Lazar"
            },
            new Entity
            {
                FirstName = "Andrew",
                LastName = "Arnautovic"
            },
            new Entity
            {
                FirstName = "Maryna",
                LastName = "Korney"
            },
            new Entity
            {
                FirstName = "Artem",
                LastName = "Losev"
            },
            new Entity
            {
                FirstName = "Kristina",
                LastName = "Mironchik"
            }
        };
    }
}
