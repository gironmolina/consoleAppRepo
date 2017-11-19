using System;
using System.Collections.Generic;
using System.Linq;
using TestGrill.Application.Interfaces;
using TestGrill.Entities;
using TestGrill.Infrastructure;
using Unity.Attributes;

namespace TestGrill.Application.Services
{
    public class GrillService : IGrillService
    {
        [Dependency]
        public IODataClient ODataClient { get; set; }

        private int[,] GrillArray { get; set; }

        public GrillService(int[,] grillArray)
        {
            GrillArray = grillArray;
        }

        public IList<Menu> GetMenu()
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
            }

            return menuList;
        }

        public void Cook(IList<Menu> menuList)
        {
            Console.WriteLine("Started to cooking");
            var menuNumber = 0;
            foreach (var menu in menuList)
            {
                menuNumber++;
                Array.Clear(GrillArray, 0, GrillArray.Length);

                var orderedGoods = menu.Goods.OrderByDescending(i => i.Length).ToList();

                int rounds;
                var result = TryGetGrillRounds(orderedGoods, out rounds);
                Console.WriteLine(result
                    ? $"Menu {menuNumber:D2} is not possible to cook"
                    : $"Menu {menuNumber:D2}: {rounds} rounds");
            }
        }

        private bool TryGetGrillRounds(IList<Goods> goods, out int rounds)
        {
            rounds = 1;
            foreach (var good in goods)
            {
                var goodWidth = good.Width;
                var goodLength = good.Length;

                if (goodWidth > GrillArray.GetLength(0) || 
                    goodLength > GrillArray.GetLength(1))
                {
                    return false;
                }

                rounds += CalculateRounds(goodWidth, goodLength);
            }

            return false;
        }

        private int CalculateRounds(int width, int length)
        {
            int posX;
            int posY;
            var xFound = TryGetPositionX(width, out posX);
            var yFound = TryGetPositionY(length, posX, out posY);
            
            if (xFound == false && yFound == false)
            {
                Array.Clear(GrillArray, 0, GrillArray.Length);
                return 1;
            }

            var position = new Position { PositionX = posX, PositionY = posY };

            PutOnGrill(position, width, length);
            ShowGrill();
            return 0;
        }

        private bool TryGetPositionX(int width, out int posX)
        {
            for (var j = 0; j < GrillArray.GetLength(1); j++)
            {
                for (var i = 0; i < GrillArray.GetLength(0); i++)
                {
                    if (GrillArray[i, j] != 0)
                    {
                        continue;
                    }

                    if (i + width > GrillArray.GetLength(0))
                    {
                        i = 0;
                        j++;
                        continue;
                    }

                    posX = i;
                    return true;
                }
            }

            posX = 0;
            return false;
        }

        private bool TryGetPositionY(int length, int posX, out int posY)
        {
            for (var y = 0; y < GrillArray.GetLength(1); y++)
            {
                if (GrillArray[posX, y] != 0)
                {
                    continue;
                }

                if (y + length > GrillArray.GetLength(1))
                {
                    break;
                }

                posY = y;
                return true;
            }

            posY = 0;
            return false;
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
