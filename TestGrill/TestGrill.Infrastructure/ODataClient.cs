using System;
using System.Collections.Generic;
using System.Net;
using TestGrill.Entities;
using TestGrill.Infrastructure.GrillServiceOData;

namespace TestGrill.Infrastructure
{
    public class ODataClient : IODataClient
    {
        public GrillMenuContext Service { get; set; }
        
        public ODataClient(string oDataUri)
        {
            var serviceUri = new Uri(oDataUri);
            var serviceCreds = new NetworkCredential("jobs@isolutions.ch", "cleancode");
            var cache = new CredentialCache { { serviceUri, "Basic", serviceCreds } };
            this.Service = new GrillMenuContext(serviceUri)
            {
                Credentials = cache
            }; 
        }

        public IList<Menu> GetMenu()
        {
            var menuList = new List<Menu>();
            foreach (var grillMenu in this.Service.GrillMenus.Expand(g => g.GrillMenuItemQuantity))
            {
                var goods = new List<Goods>();
                foreach (var grillMenuItemQuantity in grillMenu.GrillMenuItemQuantity)
                {
                    this.Service.LoadProperty(grillMenuItemQuantity, "GrillMenuItem");
                    goods.Add(new Goods
                    {
                        Quantity = grillMenuItemQuantity.Quantity,
                        Name = grillMenuItemQuantity.GrillMenuItem.Name,
                        Width = grillMenuItemQuantity.GrillMenuItem.Width,
                        Length = grillMenuItemQuantity.GrillMenuItem.Length
                    });
                }

                menuList.Add(new Menu { Goods = goods });
            }

            return menuList;
        }
    }
}