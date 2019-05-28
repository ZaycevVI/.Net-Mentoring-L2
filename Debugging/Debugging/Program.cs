using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Debugging
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new CodeGenerator();
            Console.WriteLine(generator.Generate());
        }

        private sealed class CodeGenerator
        {
            private byte[] _bytes;

            public string Generate()
            {
                var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
                var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                _bytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
                return string.Join("-", addressBytes.Select(Eval1)
                    .Select(Eval2));
            }

            private int Eval1(byte arg1, int arg2)
            {
                return arg1 ^ _bytes[arg2];
            }

            private int Eval2(int arg)
            {
                return arg < 999 ? arg * 10 : arg;
            }
        }
    }
}
