using System;
using TestGrill.Application;
using Unity;

namespace TestGrill
{
    internal class Program
    {
        private static IUnityContainer container;

        private static void Main()
        {
            container = UnityConfig.LoadContainers;
            var programStarter = container.Resolve<ProgramStarter>();
            programStarter.Run();
            Console.WriteLine();
            Console.WriteLine("Press a key to Exit");
            Console.Read();
        }
    }
}