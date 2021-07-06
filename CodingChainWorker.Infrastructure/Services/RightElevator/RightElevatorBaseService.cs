using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.RightElevator
{
    public abstract class RightElevatorBaseService : IRightElevatorService
    {
        protected readonly ILogger<RightElevatorBaseService> Logger;

        protected RightElevatorBaseService(ILogger<RightElevatorBaseService> logger)
        {
            Logger = logger;
        }

        protected abstract Task ElevateRights(string path);

        public async Task Elevate(string path)
        {
            Logger.LogInformation("Elevate rights for file {Path}", path);
            await ElevateRights(path);
            Logger.LogInformation("Rights elevated for file {Path}", path);
        }
    }
}