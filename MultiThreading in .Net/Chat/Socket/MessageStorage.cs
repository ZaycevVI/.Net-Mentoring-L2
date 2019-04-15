using System.Collections.Concurrent;

namespace SocketLibrary
{
    public class MessageStorage
    {
        public ConcurrentStack<string> Messages { get; }

        public MessageStorage()
        {
            Messages = new ConcurrentStack<string>();
        }

        public bool Push(string msg)
        {
            var success = true;

            if (Messages.Count == 10)
                success = Messages.TryPop(out var pop);

            if (success)
                Messages.Push(msg);

            return success;
        }
    }
}
