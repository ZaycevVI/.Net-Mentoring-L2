using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CRUD
{
    // У вас есть Entity, которое описывает класс пользователя, хранящийся в БД.
    // Пользователь хранит информацию об Имени, Фамилии, Возрасте. 
    // Напишите пример асинхронных CRUD операций для этого класса.
    public class UserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new ConcurrentDictionary<Guid, User>();

        public Task<bool> AddAsync(User user)
        {
            return Task.Delay(1500).ContinueWith(task => _users.TryAdd(user.Id, user));
        }

        public Task<User> RemoveAsync(Guid id)
        {
            return Task.Delay(1500).ContinueWith(task =>
            {
                _users.TryRemove(id, out var user);
                return user;
            });
        }

        public Task<bool> UpdateAsync(User user)
        {
            return Task.Delay(1500).ContinueWith(task =>
            {
                var id = user.Id;
                return _users.TryUpdate(id, user, _users[id]);
            });
        }

        public Task<User> GetAsync(Guid id)
        {
            return Task.Delay(1500).ContinueWith(task =>
            {
                _users.TryGetValue(id, out var user);
                return user;
            });
        }
    }
}
