using System;

namespace CRUD
{
    // У вас есть Entity, которое описывает класс пользователя, хранящийся в БД.
    // Пользователь хранит информацию об Имени, Фамилии, Возрасте. 
    // Напишите пример асинхронных CRUD операций для этого класса.
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
    }
}
