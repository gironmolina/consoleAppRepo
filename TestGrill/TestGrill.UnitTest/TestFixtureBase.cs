using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Unity;

namespace TestGrill.UnitTest
{
    [ExcludeFromCodeCoverage]
    public abstract class TestFixtureBase
    {
        /// <summary>
        /// The container.
        /// </summary>
        protected readonly IUnityContainer Container = TestAssemblyInitializer.container;

        /// <summary>
        /// The test set up.
        /// </summary>
        [SetUp]
        public virtual void TestSetUp()
        {
        }

        /// <summary>
        /// The tear down.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}
