using System;
using TestGrill.Application.Interfaces;
using Unity.Attributes;

namespace TestGrill.Application
{
    public class ProgramStarter : IProgramStarter
    {
        [Dependency]
        public IGrillService GrillService { get; set; }
        
        public void StartGrill()
        {
            var menuList = GrillService.GetMenu();
            GrillService.Cook(menuList);
        }
    }
}