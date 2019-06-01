using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Newtonsoft.Json;

namespace WindowsService
{
    public class LoggingProxy<T> : RealProxy
    {
        private readonly T _instance;

        private LoggingProxy(T instance)
            : base(typeof(T))
        {
            _instance = instance;
        }

        public static T Create(T instance)
        {
            return (T)new LoggingProxy<T>(instance).GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;

            try
            {
                var args = methodCall.InArgs.Select(Serialize);
                Console.WriteLine("=================================");
                Console.WriteLine($"[{DateTime.Now}]: Before invoke: {method.Name}.");

                var i = 1;
                foreach (var arg in args)
                {
                    Console.WriteLine($"Argument {i++}: {arg}");
                }
                
                var result = method.Invoke(_instance, methodCall.InArgs);
                Console.WriteLine($"After invoke: {method.Name}");
                Console.WriteLine($"Returning result: {Serialize(result)}");
                Console.WriteLine("=================================");
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                if (e is TargetInvocationException && e.InnerException != null)
                {
                    return new ReturnMessage(e.InnerException, (IMethodCallMessage)msg);
                }

                return new ReturnMessage(e, (IMethodCallMessage)msg);
            }
        }

        private string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception)
            {
                return "Not Serializer";
            }
        }
    }
}
