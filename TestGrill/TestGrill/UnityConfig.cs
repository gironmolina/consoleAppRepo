using System;
using Unity;
using System.Configuration;
using TestGrill.Application.Interfaces;
using TestGrill.Application.Services;
using TestGrill.Infrastructure;
using Unity.Injection;

namespace TestGrill
{
    public static class UnityConfig
    {
        private static readonly int[,] GrillArray = new int[20, 30];

        private static readonly Lazy<IUnityContainer> UnityContainer = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Configured Unity LoadContainers.
        /// </summary>
        public static IUnityContainer LoadContainers => UnityContainer.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IODataClient), new ODataClient(ConfigurationManager.AppSettings["ODataAPIUrl"]));
            container.RegisterType<IGrillService, GrillService>(new InjectionConstructor(GrillArray));
        }
    }
}
