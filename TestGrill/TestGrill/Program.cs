using System;
using System.Collections.Generic;
using System.Net;
using TestGrill.GrillServiceOData;

namespace TestGrill
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceUri = new Uri("http://grillassessmentservice.cloudapp.net/GrillMenuService.svc");
            var serviceCreds = new NetworkCredential("jobs@isolutions.ch", "cleancode");
            var cache = new CredentialCache { { serviceUri, "Basic", serviceCreds } };
            var service = new GrillMenuContext(serviceUri)
            {
                Credentials = cache
            };

            var grillArray = new int[19, 29];
            //var menus = new List<Menu>();

            //foreach (var grillMenu in service.GrillMenus.Expand(g => g.GrillMenuItemQuantity))
            //{
            //    var goods = new List<Goods>();

            //    foreach (var grillMenuItemQuantity in grillMenu.GrillMenuItemQuantity)
            //    {

            //        service.LoadProperty(grillMenuItemQuantity, "GrillMenuItem");
            //        goods.Add(new Goods
            //        {
            //            Quantity = grillMenuItemQuantity.Quantity,
            //            Name = grillMenuItemQuantity.GrillMenuItem.Name,
            //            Width = grillMenuItemQuantity.GrillMenuItem.Width,
            //            Length = grillMenuItemQuantity.GrillMenuItem.Length
            //        });
            //    }

            //    menus.Add(new Menu { Goods = goods });
            //}

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grillArray[i, j] = 1;
                }
            }

            for (int i = 0; i < grillArray.GetLength(0); i++)
            {
                for (int j = 0; j < grillArray.GetLength(1); j++)
                {
                    var s = grillArray[i, j];
                    Console.Write(s);
                }
                Console.WriteLine();
            }



            Console.ReadLine();
        }
    }
}
