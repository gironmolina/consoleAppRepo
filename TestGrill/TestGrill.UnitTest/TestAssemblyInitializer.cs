using NUnit.Framework;
using Rhino.Mocks;
using TestGrill.Infrastructure;
using Unity;

namespace TestGrill.UnitTest
{
    [SetUpFixture]
    public class TestAssemblyInitializer
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IUnityContainer Container { get; private set; } = new UnityContainer();

        /// <summary>
        /// Resets the external dependencies.
        /// </summary>
        public static void ResetExternalDependencies()
        {
            Container.RegisterInstance(typeof(IODataClient), MockRepository.GenerateMock<IODataClient>());
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
