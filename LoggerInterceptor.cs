using System;
using System.Diagnostics;
using System.IO;
using Castle.DynamicProxy;

namespace spell_check
{
    public class LoggerInterceptor: IInterceptor
    {
        public LoggerInterceptor()
        {

        }

        public void Intercept(IInvocation invocation)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            invocation.Proceed();
            stopWatch.Stop();
            WriteCharacters(stopWatch.Elapsed, invocation.Method.Name);
        }

        private async void WriteCharacters(TimeSpan time, string methodName)
        {
            using (StreamWriter writer = File.CreateText("newfile.txt"))
            {
                await writer.WriteAsync($"{methodName} -- {time}");
            }
        }
    }
}