using System;
using System.Collections.Generic;
using System.Linq;
using TestGrill.Application.Interfaces;
using TestGrill.Entities;
using TestGrill.Infrastructure;
using Unity.Attributes;

namespace TestGrill.Application
{
    public class ProgramStarter : IProgramStarter
    {
        private int[,] GrillArray = new int[19, 29];

        [Dependency]
        public IODataClient ODataClient { get; set; }

        public void StartGrill()
        {
            var menuList = GetMenu();

            Cook(menuList);

            Console.ReadLine();
        }

        private List<Menu> GetMenu()
        {
            var menuList = new List<Menu>();
            foreach (var grillMenu in ODataClient.Service.GrillMenus.Expand(g => g.GrillMenuItemQuantity))
            {
                var goods = new List<Goods>();
                foreach (var grillMenuItemQuantity in grillMenu.GrillMenuItemQuantity)
                {
                    ODataClient.Service.LoadProperty(grillMenuItemQuantity, "GrillMenuItem");
                    goods.Add(new Goods
                    {
                        Quantity = grillMenuItemQuantity.Quantity,
                        Name = grillMenuItemQuantity.GrillMenuItem.Name,
                        Width = grillMenuItemQuantity.GrillMenuItem.Width,
                        Length = grillMenuItemQuantity.GrillMenuItem.Length
                    });
                }

                menuList.Add(new Menu { Goods = goods });

                // TODO Delete this
                break;
            }

            return menuList;
        }

        private void Cook(List<Menu> menuList)
        {
            foreach (var menu in menuList)
            {
                var orderedGoods = menu.Goods
                    .OrderBy(i => i.Length)
                    .ThenBy(i => i.Width);

                foreach (var good in orderedGoods)
                {
                    var goodWidth = good.Width;
                    var goodLength = good.Length;

                    var spot = FindFreeSpot(goodWidth, goodLength);

                    PutOnGrill(spot, goodWidth, goodLength);

                    ShowGrill();
                }
            }
        }

        private Position FindFreeSpot(int width, int length)
        {
            var posX = 0;
            var posY = 0;

            // empiezo en X = 0
            for (var x = 0; x < GrillArray.GetLength(0); x++)
            {
                // busco primer X igual a 0
                if (GrillArray[x, 0] == 0)
                {
                    // comparo que el ancho del item sea menor al max ancho disponible
                    if (x + width > GrillArray.GetLength(0))
                    {
                        // TODO
                    }
                    // tengo X
                    posX = x;
                    break;
                }
            }

            // empiezo en Y = 0
            for (var y = 0; y < GrillArray.GetLength(1); y++)
            {
                // busco primer Y igual a 0
                if (GrillArray[y, 0] == 0)
                {
                    // comparo que la altura del item sea menor al max altura disponible
                    if (y + length > GrillArray.GetLength(0))
                    {
                        // TODO
                    }
                    // tengo Y
                    posY = y;
                    break;
                }
            }

            return new Position { PositionX = posX, PositionY = posY };
        }

        private void PutOnGrill(Position spot, int goodWidth, int goodLength)
        {

            for (var y = spot.PositionY; y < goodLength; y++)
            {
                for (var x = spot.PositionX; x < goodWidth; x++)
                {
                    GrillArray[y, x] = 1;
                }
            }
        }

        private void ShowGrill()
        {
            for (var i = 0; i < GrillArray.GetLength(0); i++)
            {
                for (var j = 0; j < GrillArray.GetLength(1); j++)
                {
                    var s = GrillArray[i, j];
                    Console.Write(s);
                }
                Console.WriteLine();
            }
        }
    }
}
