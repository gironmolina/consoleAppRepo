using System;
using System.Collections.Generic;
using System.Linq;
using TestGrill.Application.Interfaces;
using TestGrill.Entities;
using TestGrill.Infrastructure;
using Unity.Attributes;

namespace TestGrill.Application.Services
{
    public class Grill : IGrill
    {
        private int rounds = 0;

        [Dependency]
        public IODataClient ODataClient { get; set; }

        private int[,] GrillArray { get; set; }

        public Grill(int[,] grillArray)
        {
            GrillArray = grillArray;
        }

        public IEnumerable<Menu> GetMenu()
        {
            Console.WriteLine("Retrieving Menu List");
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
                //break;
            }

            return menuList;
        }

        public void Cook(IEnumerable<Menu> menuList)
        {
            Console.WriteLine("Started to cooking");
            var menuNumber = 1;
            var msg = string.Empty;
            foreach (var menu in menuList)
            {
                rounds = 1;
                Array.Clear(GrillArray, 0, GrillArray.Length);

                var orderedGoods = menu.Goods
                    .OrderByDescending(i => i.Length);

                foreach (var good in orderedGoods)
                {
                    var goodWidth = good.Width;
                    var goodLength = good.Length;

                    if (goodWidth > GrillArray.GetLength(0) || goodLength > GrillArray.GetLength(1))
                    {
                        msg = $"Menu {menuNumber:D2} is not possible to Cook.";
                        break;
                    }

                    CalculateCookRounds(goodWidth, goodLength);
                    msg = $"Menu {menuNumber:D2}: {rounds} rounds";
                }

                Console.WriteLine(msg);
                menuNumber++;
            }
        }

        private int CalculateCookRounds(int width, int length)
        {
            //var rounds = 1;

            while (true)
            {
                var posX = GetPositionX(width);
                var posY = GetPositionY(length, posX);

                if (posY == null)
                {
                    Array.Clear(GrillArray, 0, GrillArray.Length);
                    rounds++;
                    continue;
                }

                var position = new Position { PositionX = posX, PositionY = posY.Value };

                PutOnGrill(position, width, length);
                // TODO
                //ShowGrill();
                break;
            }

            return rounds;
        }

        private int GetPositionX(int width)
        {
            var posX = 0;

            for (var x = 0; x < GrillArray.GetLength(0); x++)
            {
                if (GrillArray[x, 0] != 0)
                {
                    continue;
                }

                if (x + width > GrillArray.GetLength(0))
                {
                    posX = 0;
                    break;
                }

                posX = x;
                break;
            }

            return posX;
        }

        private int? GetPositionY(int length, int posX)
        {
            int? posY = null;
            for (var y = 0; y < GrillArray.GetLength(1); y++)
            {
                if (GrillArray[posX, y] != 0)
                {
                    continue;
                }

                if (y + length > GrillArray.GetLength(1))
                {
                    //posY = null;
                    break;
                }

                posY = y;
                break;
            }

            return posY;
        }
        
        private void PutOnGrill(Position spot, int goodWidth, int goodLength)
        {
            for (var x = 0; x < goodWidth; x++)
            {
                for (var y = 0; y < goodLength; y++)
                {
                    GrillArray[spot.PositionX + x, spot.PositionY + y] = 1;
                }
            }
        }
        
        private void ShowGrill()
        {
            for (var j = 0; j < GrillArray.GetLength(1); j++)
            {
                for (var i = 0; i < GrillArray.GetLength(0); i++)
                {
                    var s = GrillArray[i, j];
                    Console.Write(s);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
