using System.Collections.Generic;
using TestGrill.Entities;
using TestGrill.Infrastructure.GrillServiceOData;

namespace TestGrill.Infrastructure
{
    public interface IODataClient
    {
        GrillMenuContext Service { get; set; }

        IList<Menu> GetMenu();
    }
}