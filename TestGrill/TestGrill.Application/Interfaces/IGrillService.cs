using System.Collections.Generic;
using TestGrill.Entities;

namespace TestGrill.Application.Interfaces
{
    public interface IGrillService
    {
        IList<Menu> GetMenu();

        void Cook(IList<Menu> menuList);
    }
}