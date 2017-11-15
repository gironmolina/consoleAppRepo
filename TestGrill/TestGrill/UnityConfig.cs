using System;
using Unity;
using System.Configuration;
using TestGrill.Infrastructure;

namespace TestGrill
{
    public static class UnityConfig
    {
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
        }
    }
}
