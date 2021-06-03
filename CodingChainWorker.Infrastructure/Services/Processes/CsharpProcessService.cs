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
        private readonly IAppDataSettings _appDataSettings;
        private readonly ICSharpExecutionSettings _cSharpExecutionSettings;
        private readonly ILogger<CsharpProcessService> _logger;

        private  string TemplatePath =>Path.GetFullPath(Path.Combine(_appDataSettings.BasePath,
            _appDataSettings.TemplatesPath,
            _cSharpExecutionSettings.TemplatePath));

        protected override string TestsFilePath =>
            Path.Combine(TemplatePath, $"{_cSharpExecutionSettings.BaseTestFileName}.cs");

        private const string ProcessName = "dotnet ";
        private const string TestCommand = "test -v n";

        public CsharpProcessService(IAppDataSettings appDataSettings, ICSharpExecutionSettings cSharpExecutionSettings, ILogger<CsharpProcessService> logger)
        {
            _appDataSettings = appDataSettings;
            _cSharpExecutionSettings = cSharpExecutionSettings;
            _logger = logger;
        }

        public override async Task ExecuteParticipation(CSharpParticipationTestingAggregate participation)
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
                    Arguments = $"{TestCommand} {TemplatePath}"
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
                _logger.LogDebug("{Output}",e.Data);
            };
            process.Exited += (o, e) =>
            {
                _logger.LogDebug("Process ended");
            };
            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
        }
    }
}