using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using TestGrill.Application.Interfaces;
using TestGrill.Entities;
using Unity;

namespace TestGrill.UnitTest.Services
{
    public class GrillTest : TestFixtureBase
    {
        public override void TestSetUp()
        {
            base.TestSetUp();
            TestAssemblyInitializer.ResetExternalDependencies();
            // Mocked Grill Service
            Container.RegisterInstance(typeof(IGrillService), MockRepository.GenerateMock<IGrillService>());
            // Container.RegisterType<IGrillService, GrillService>(new InjectionConstructor(new int[20, 30]));
        }

        [Test]
        public void Sample_Test()
        {
            var grillService = this.Container.Resolve<IGrillService>();
            grillService.Stub(x => x.GetMenu()).Return(new List<Menu>());
            Assert.Inconclusive("Test Sample");
        }
    }
}
