using System.Diagnostics;
using System.IO;
using System.Text;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class WindowsTypescriptProcessService : ProcessService, ITypescriptProcessService
    {
        private const string TestCommand = "test";
        private readonly ITypescriptExecutionSettings _typescriptExecutionSettings;
        protected override FileInfo TestsFilePath =>
            new(Path.Combine(TemplateDirectoryPath.FullName,
                $"{_typescriptExecutionSettings.BaseTestFileName}.spec.ts"));

        protected override string ProcessArguments =>
            $"/c npm --prefix {TemplateDirectoryPath} run {TestCommand} & exit";

        protected override string ProcessName => "cmd.exe";
        protected sealed override ILogger<ProcessService> Logger { get; set; }

        public WindowsTypescriptProcessService(IDirectoryService directoryService,
            ILogger<WindowsTypescriptProcessService> logger, ITypescriptExecutionSettings typescriptExecutionSettings) : base(
            directoryService)
        {
            Logger = logger;
            _typescriptExecutionSettings = typescriptExecutionSettings;
        }

        protected override Process GetProcess(ParticipationAggregate partipation)
        {
            return new()
            {
                StartInfo =
                {
                    FileName = ProcessName,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    Arguments = ProcessArguments
                },
                EnableRaisingEvents = true
            };
        }
    }
}