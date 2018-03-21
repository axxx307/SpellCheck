using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
            using (FileStream stream = new FileStream(@"Performance.txt", FileMode.Append, FileAccess.Write, FileShare.None, bufferSize:4096, useAsync: true))
            {
                var text = $"{methodName} -- {time} \n";
                await stream.WriteAsync(Encoding.ASCII.GetBytes(text), 0, text.Length);
            }
        }
    }
}