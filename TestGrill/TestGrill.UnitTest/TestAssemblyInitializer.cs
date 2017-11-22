using NUnit.Framework;
using Rhino.Mocks;
using TestGrill.Application.Interfaces;
using TestGrill.Application.Services;
using TestGrill.Infrastructure;
using Unity;
using Unity.Injection;

namespace TestGrill.UnitTest
{
    [SetUpFixture]
    public class TestAssemblyInitializer
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IUnityContainer container { get; private set; } = new UnityContainer();

        /// <summary>
        /// Resets the external dependencies.
        /// </summary>
        public static void ResetExternalDependencies()
        {
            container.RegisterInstance(typeof(IODataClient), MockRepository.GenerateMock<IODataClient>());
            container.RegisterType<IGrillService, GrillService>(new InjectionProperty("GrillArray", new int[50, 30]));
        }

        /// <summary>
        /// Assemblies the set up.
        /// </summary>
        [OneTimeSetUp]
        public void AssemblySetUp()
        {
            ResetExternalDependencies();
        }
    }
}
