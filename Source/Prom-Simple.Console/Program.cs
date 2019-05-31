using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Prom_Simple;

namespace Krowiorsch
{
    class Program
    {
        static void Main(string[] args)
        {
            var serialiser = new SimplePrometheusTextSerializer<AppState>().Initialize("Test", new Dictionary<string, string>()
            {
                { "env", "testing"}
            });

            var appState = new AppState(DateTimeOffset.Now);

            appState.CountTotal += 1;

            Console.Write(serialiser.Serialize(appState));

            //StringBuilder sb = new StringBuilder();
            //StringWriter writer = new StringWriter(sb);
            //Console.SetOut(writer);
            ////Thread.Sleep(TimeSpan.FromSeconds(20));

            //sb.Append(serialiser.Serialize(appState));

            //Console.WriteLine("Hello World!");
        }
    }
}
