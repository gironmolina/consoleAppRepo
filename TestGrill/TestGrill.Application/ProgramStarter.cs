using TestGrill.Application.Interfaces;

namespace TestGrill.Application
{
    public class ProgramStarter : IProgramStarter
    {
        private IGrillService GrillService { get; set; }

        public ProgramStarter(IGrillService grillService)
        {
            this.GrillService = grillService;
        }

        public void Run()
        {
            var menuList = this.GrillService.GetMenu();
            this.GrillService.Cook(menuList);
        }
    }
}