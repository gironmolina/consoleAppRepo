using System;
using System.Collections.Generic;
using TestGrill.Application.Interfaces;
using TestGrill.Infrastructure;
using Unity.Attributes;

namespace TestGrill.Application
{
    public class ProgramStarter : IProgramStarter
    {
        private new int[,] GrillArray = new int[19, 29];

        [Dependency]
        public IODataClient oDataClient { get; set; }

        public void StartGrill()
        {
            var service = oDataClient.Service;
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

        private bool IsAvalaible(List<Goods> goods)
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

        private void PutOnGrill()
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    GrillArray[i, j] = 1;
                }
            }
        }

        private void ShowGrill()
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
