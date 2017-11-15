using System;
using System.Collections.Generic;
using System.Net;
using TestGrill.GrillServiceOData;

namespace TestGrill
{
    class Program
    {
        public new static int[,] GrillArray = new int[19, 29];

        static void Main(string[] args)
        {
            var serviceUri = new Uri("http://grillassessmentservice.cloudapp.net/GrillMenuService.svc");
            var serviceCreds = new NetworkCredential("jobs@isolutions.ch", "cleancode");
            var cache = new CredentialCache { { serviceUri, "Basic", serviceCreds } };
            var service = new GrillMenuContext(serviceUri)
            {
                Credentials = cache
            };

            var menus = new List<Menu>();
            foreach (var grillMenu in service.GrillMenus.Expand(g => g.GrillMenuItemQuantity))
            {
                var goods = new List<Goods>();

                foreach (var grillMenuItemQuantity in grillMenu.GrillMenuItemQuantity)
                {

                    service.LoadProperty(grillMenuItemQuantity, "GrillMenuItem");
                    goods.Add(new Goods
                    {
                        Quantity = grillMenuItemQuantity.Quantity,
                        Name = grillMenuItemQuantity.GrillMenuItem.Name,
                        Width = grillMenuItemQuantity.GrillMenuItem.Width,
                        Length = grillMenuItemQuantity.GrillMenuItem.Length
                    });
                }

                menus.Add(new Menu { Goods = goods });
                break;
            }

            // Evaluate space
            var isAvalaible = IsAvalaible(menus[0].Goods);

            // Paint
            PutOnGrill();

            ShowGrill();

            Console.ReadLine();
        }

        private static bool IsAvalaible(List<Goods> goods)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (GrillArray[i, j] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static void PutOnGrill()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    GrillArray[i, j] = 1;
                }
            }
        }

        private static void ShowGrill()
        {
            for (int i = 0; i < GrillArray.GetLength(0); i++)
            {
                for (int j = 0; j < GrillArray.GetLength(1); j++)
                {
                    var s = GrillArray[i, j];
                    Console.Write(s);
                }
                Console.WriteLine();
            }
        }
    }
}

