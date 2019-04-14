using System;
using System.Text;

namespace SocketLibrary
{
    public class DataConverter
    {
        public static ArraySegment<byte> GetBytes(string data)
        {
            return new ArraySegment<byte>(Encoding.ASCII.GetBytes(data));
        }

        public static string GetString(byte[] buffer, int count)
        {
            return Encoding.ASCII.GetString(buffer, 0, count);
        }
    }
}
