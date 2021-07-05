using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.RightElevator
{
    public class WindowsRightElevatorService : RightElevatorBaseService
    {
        public WindowsRightElevatorService(ILogger<WindowsRightElevatorService> logger) : base(logger)
        {
        }

        protected override Task ElevateRights(string path)
        {
            Logger.LogInformation("Do nothing for right elevation on windows ");
            return Task.CompletedTask;
        }
    }
}