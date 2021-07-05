using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.RightElevator
{
    public class MacRightElevatorService : RightElevatorBaseService
    {
        public MacRightElevatorService(ILogger<MacRightElevatorService> logger) : base(logger)
        {
        }
        protected override Task ElevateRights(string path)
        {
            Logger.LogInformation("Do nothing for right elevation on mac os ");
            return Task.CompletedTask;
        }
    }
}