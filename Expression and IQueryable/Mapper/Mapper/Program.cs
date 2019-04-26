using System;

namespace MapperSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Person, Cat>();
            var person = new Person
            {
                Name = "Vasilii",
                Age = 13,
                CustomField = "dsdsds",
                Sex = Sex.Male
            };

            var cat = mapper.Map(person);

            Print(cat, person);

            person.Name = "qwasdssssssssssssssssssssssssssssssss";
            person.Age = 1212112121;
            cat = mapper.Map(person);

            Console.WriteLine("==================================");
            Console.WriteLine("==================================");
            Console.WriteLine("==================================");


            Print(cat, person);
        }

        static void Print(Cat cat, Person person)
        {
            Console.WriteLine("Person instance:");
            Console.WriteLine($"\tName: {person.Name}");
            Console.WriteLine($"\tAge: {person.Age}");
            Console.WriteLine($"\tSex: {person.Sex}");
            Console.WriteLine($"\tCustomField: {person.CustomField}");
            Console.WriteLine("==================================");
            Console.WriteLine("Cat instance:");
            Console.WriteLine($"\t\tName: {cat.Name}");
            Console.WriteLine($"\t\tAge: {cat.Age}");
            Console.WriteLine($"\t\tSex: {cat.Sex}");
            Console.WriteLine($"\t\tCustomField111111: {cat.CustomField111111}");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string CustomField { get; set; }
    }

    public class Cat
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string CustomField111111 { get; set; }

    }

    public enum Sex
    {
        Male,
        Female
    }
}
