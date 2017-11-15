using TestGrill.Infrastructure.GrillServiceOData;

namespace TestGrill.Infrastructure
{
    public interface IODataClient
    {
        GrillMenuContext Service { get; set; }
    }
}