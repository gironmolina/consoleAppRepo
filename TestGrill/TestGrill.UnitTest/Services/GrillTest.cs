using System.Collections.Generic;
using NUnit.Framework;
using TestGrill.Application.Interfaces;
using TestGrill.Application.Services;
using TestGrill.Entities;
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
            var menuList = new List<Goods>
            {
                new GoodsBuilder().Name("Veal").Quantity(10).Width(4).Length(8),
                new GoodsBuilder().Name("Paprika Sausage").Quantity(40).Width(3).Length(6)
            };

            var menuMock = new List<Menu> { new Menu { Goods = menuList } };
            var sut = CreateGrillService(new int[50, 30]);

            // act
            sut.Cook(menuMock);

            // assert
            Assert.Inconclusive("Test Sample");
        }

        private IGrillService CreateGrillService(int[,] grillArray)
        {
            this.Container.RegisterType<IGrillService, GrillService>(new InjectionProperty("GrillArray", grillArray));
            return this.Container.Resolve<IGrillService>();
        }
    }
}
