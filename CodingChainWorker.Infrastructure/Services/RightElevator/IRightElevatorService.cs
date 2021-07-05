using System.Threading.Tasks;

namespace CodingChainApi.Infrastructure.Services.RightElevator
{
    public interface IRightElevatorService
    {
        Task Elevate(string path);
    }
}