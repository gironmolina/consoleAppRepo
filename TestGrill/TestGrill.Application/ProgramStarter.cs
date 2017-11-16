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
        //private int[,] GrillArray = new int[19, 29];
        private int[,] GrillArray = new int[6, 10];

        [Dependency]
        public IODataClient ODataClient { get; set; }

        public void StartGrill()
        {
            var menuList = this.GetMenu();

            this.Cook(menuList);

            Console.ReadLine();
        }

        private List<Menu> GetMenu()
        {
            var menuList = new List<Menu>();
            foreach (var grillMenu in this.ODataClient.Service.GrillMenus.Expand(g => g.GrillMenuItemQuantity))
            {
                var goods = new List<Goods>();
                foreach (var grillMenuItemQuantity in grillMenu.GrillMenuItemQuantity)
                {
                    this.ODataClient.Service.LoadProperty(grillMenuItemQuantity, "GrillMenuItem");
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
                    .OrderByDescending(i => i.Length);

                foreach (var good in orderedGoods)
                {
                    var goodWidth = good.Width;
                    var goodLength = good.Length;

                    var spot = this.FindFreeSpot(goodWidth, goodLength);
                    
                    this.PutOnGrill(spot, goodWidth, goodLength);

                    this.ShowGrill();
                }
            }
        }

        private Position FindFreeSpot(int width, int length)
        {
            var rounds = 0;
            var posX = 0;
            int posY = 0;
            
            // empiezo en X = 0
            for (var x = 0; x < this.GrillArray.GetLength(0); x++)
            {
                // busco primer X igual a 0
                if (this.GrillArray[x, 0] == 0)
                {
                    // comparo que el ancho del item sea menor al max ancho disponible
                    if (x + width > this.GrillArray.GetLength(0))
                    {
                        // TODO
                        posX = 0;
                        break;
                    }
                    else
                    {
                        // tengo X
                        posX = x;
                        break;
                    }
                    
                }
            }

            // empiezo en Y = 0
            for (var y = 0; y < this.GrillArray.GetLength(1); y++)
            {
                // busco primer Y igual a 0
                if (this.GrillArray[posX, y] == 0)
                {
                    // comparo que la altura del item sea menor al max altura disponible
                    if (y + length > this.GrillArray.GetLength(1))
                    {
                        rounds++;
                        break;
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
            for (var x = 0; x < goodWidth; x++)
            {
                for (var y = 0; y < goodLength; y++)
                {
                    this.GrillArray[spot.PositionX + x, spot.PositionY + y] = 1;
                }
            }
        }

        private void ShowGrill()
        {
            for (int j = 0; j < this.GrillArray.GetLength(1); j++)
            {
                for (int i = 0; i < this.GrillArray.GetLength(0); i++)
                {
                    var s = this.GrillArray[i, j];
                    Console.Write(s);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
