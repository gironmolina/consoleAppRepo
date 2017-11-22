using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using TestGrill.Application.Interfaces;
using TestGrill.Application.Services;
using TestGrill.Entities;
using TestGrill.Infrastructure;
using TestGrill.TestUtil.Builders;
using Unity;
using Unity.Injection;

namespace TestGrill.UnitTest.Services
{
    public class GrillTest : TestFixtureBase
    {
        public override void TearDown()
        {
            TestAssemblyInitializer.ResetExternalDependencies();
        }

        [Test]
        public void Sample_Test()
        {
            // arrange
            this.Container.RegisterType<IGrillService, GrillService>(new InjectionProperty("GrillArray", new int[50, 30]));
            var grillService = this.Container.Resolve<IGrillService>();

            var menuList = new List<Goods>
            {
                new GoodsBuilder().Name("Veal").Quantity(10).Width(4).Length(8),
                new GoodsBuilder().Name("Paprika Sausage").Quantity(40).Width(3).Length(6)
            };

            var menuMock = new List<Menu> { new Menu { Goods = menuList } };
            grillService.Stub(x => x.GetMenu()).Return(menuMock);
            var sut = this.CreateGrillService();

            // act
            sut.Cook(menuMock);

            // assert
            //Assert.AreEqual(3, clientDto.Count);
            Assert.Inconclusive("Test Sample");
        }

        private GrillService CreateGrillService()
        {
            var sut = new GrillService(this.Container.Resolve<IODataClient>());
            return sut;
        }
    }
}
