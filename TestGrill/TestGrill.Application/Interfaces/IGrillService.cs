using System.Collections.Generic;
using TestGrill.Entities;

namespace TestGrill.Application.Interfaces
{
    public interface IGrillService
    {
        IEnumerable<Menu> GetMenu();

        void Cook(IEnumerable<Menu> menuList);
    }
}