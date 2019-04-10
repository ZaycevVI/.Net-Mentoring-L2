using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    public class Calculator
    {
        private bool _stateChanged;

        public Calculator(ulong upperLimit)
        {
            _upperLimit = upperLimit;
        }

        private ulong _upperLimit;


        public ulong UpperLimit
        {
            get => _upperLimit;
            set
            {
                _upperLimit = value;
                _stateChanged = true;
            }
        }

        public Task<ulong> CalculateAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Calculation was started with upper limit = {UpperLimit}.");

                var result = Calculate(UpperLimit);
                _stateChanged = false;

                Console.WriteLine("Calculation was finished.");

                return result;
            });
        }

        private ulong Calculate(ulong upperLimit)
        {
            ulong result = 0;

            for (ulong i = 0; i < upperLimit; i++)
            {
                Thread.Sleep(100);
                if (_stateChanged)
                {
                    Console.WriteLine($"Calculation was restarted with upper new limit = {UpperLimit}.");
                    _stateChanged = false;
                    result = Calculate(UpperLimit);
                    break;
                }

                result += 1;
            }

            return result;
        }
    }
}