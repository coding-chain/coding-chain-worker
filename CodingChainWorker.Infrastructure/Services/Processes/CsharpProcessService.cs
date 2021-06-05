using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution.OOP.CSharp;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class CsharpProcessService : ProcessService<CSharpParticipationTestingAggregate>
    {
        private readonly ICSharpExecutionSettings _cSharpExecutionSettings;
        private readonly ILogger<CsharpProcessService> _logger;


        protected override FileInfo TestsFilePath =>
            new(Path.Combine(TemplateDirectoryPath.FullName,
                $"{_cSharpExecutionSettings.BaseTestFileName}.cs"));

        private const string ProcessName = "dotnet ";
        private const string TestCommand = "test -v n";

        public CsharpProcessService(ICSharpExecutionSettings cSharpExecutionSettings,
            ILogger<CsharpProcessService> logger, IDirectoryService directoryService) : base(directoryService)
        {
            _cSharpExecutionSettings = cSharpExecutionSettings;
            _logger = logger;
        }

        protected override async Task ExecuteParticipation(CSharpParticipationTestingAggregate participation)
        {
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = ProcessName,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    Arguments = $"{TestCommand} {TemplateDirectoryPath}"
                }
            };
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += (o, e) =>
            {
                participation.AddError(e.Data ?? "");
                _logger.LogDebug("{Error}", e.Data);
            };
            process.OutputDataReceived += (o, e) =>
            {
                participation.AddOutput(e.Data ?? "");
                _logger.LogDebug("{Output}", e.Data);
            };
            process.Exited += (o, e) => { _logger.LogDebug("Process ended"); };
            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
        }
    }
}