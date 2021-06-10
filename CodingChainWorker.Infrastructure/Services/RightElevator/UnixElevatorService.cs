using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.RightElevator
{
    public class UnixElevatorService : RightElevatorBaseService
    {
        public UnixElevatorService(ILogger<UnixElevatorService> logger) : base(logger)
        {
        }
        protected override Task ElevateRights(string path)
        {
            
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = "chmod",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    Arguments = $"-R 755 {path}"
                },
                EnableRaisingEvents = true
            };
            process.Start();
            process.WaitForExit();
            return Task.CompletedTask;
        }



    }
}