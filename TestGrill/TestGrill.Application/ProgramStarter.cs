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
        // private int[,] GrillArray = new int[19, 29];
        private int[,] GrillArray = new int[10, 10];

        [Dependency]
        public IODataClient ODataClient { get; set; }

        public void StartGrill()
        {
            var menuList = this.GetMenu();

            this.Cook(menuList);

            Console.ReadLine();
        }

        private IEnumerable<Menu> GetMenu()
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
                //break;
            }

            return menuList;
        }

        private void Cook(IEnumerable<Menu> menuList)
        {
            var menuNumber = 1;
            var msg = string.Empty;
            foreach (var menu in menuList)
            {
                Array.Clear(this.GrillArray, 0, this.GrillArray.Length);

                var orderedGoods = menu.Goods
                    .OrderByDescending(i => i.Length);

                foreach (var good in orderedGoods)
                {
                    var goodWidth = good.Width;
                    var goodLength = good.Length;

                    if (goodWidth > this.GrillArray.GetLength(0) || goodLength > this.GrillArray.GetLength(1))
                    {
                        msg = $"Menu {menuNumber} is not possible to Cook.";
                        break;
                    }

                    var rounds = this.CalculateCookRounds(goodWidth, goodLength);
                    msg = $"Menu {menuNumber} : {rounds} rounds";
                }

                Console.WriteLine(msg);
                menuNumber++;
            }
        }

        private int CalculateCookRounds(int width, int length)
        {
            var rounds = 1;

            while (true)
            {
                var posX = this.GetPositionX(width);

                var posY = this.GetPositionY(length, posX);

                if (posY == null)
                {
                    Array.Clear(this.GrillArray, 0, this.GrillArray.Length);
                    rounds++;
                    continue;
                }

                var position = new Position { PositionX = posX, PositionY = posY.Value };

                this.PutOnGrill(position, width, length);
                
                // TODO
                //this.ShowGrill();

                break;
            }

            return rounds;
        }

        private int GetPositionX(int width)
        {
            var posX = 0;

            for (var x = 0; x < this.GrillArray.GetLength(0); x++)
            {
                if (this.GrillArray[x, 0] != 0)
                {
                    continue;
                }

                if (x + width > this.GrillArray.GetLength(0))
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
            int? posY = 0;
            for (var y = 0; y < this.GrillArray.GetLength(1); y++)
            {
                if (this.GrillArray[posX, y] != 0)
                {
                    continue;
                }

                if (y + length > this.GrillArray.GetLength(1))
                {
                    posY = null;
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
                    this.GrillArray[spot.PositionX + x, spot.PositionY + y] = 1;
                }
            }
        }

        private void ShowGrill()
        {
            for (var j = 0; j < this.GrillArray.GetLength(1); j++)
            {
                for (var i = 0; i < this.GrillArray.GetLength(0); i++)
                {
                    var s = this.GrillArray[i, j];
                    Console.Write(s);
                }

                Console.WriteLine();
            }
            
            Console.WriteLine();
        }
    }
}
