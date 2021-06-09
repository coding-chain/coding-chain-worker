using System.IO;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class TypescriptProcessService : ProcessService
    {
        private const string TestCommand = "test";
        private readonly ITypescriptExecutionSettings _typescriptExecutionSettings;

        public TypescriptProcessService(IDirectoryService directoryService,
            ILogger<TypescriptProcessService> logger, ITypescriptExecutionSettings typescriptExecutionSettings) : base(
            directoryService)
        {
            Logger = logger;
            _typescriptExecutionSettings = typescriptExecutionSettings;
        }

        protected override FileInfo TestsFilePath =>
            new(Path.Combine(TemplateDirectoryPath.FullName,
                $"{_typescriptExecutionSettings.BaseTestFileName}.spec.ts"));

        protected override string ProcessArguments =>
            $" npm --prefix {TemplateDirectoryPath} run {TestCommand}";

        protected override string ProcessName => "bash";
        protected sealed override ILogger<ProcessService> Logger { get; set; }
    }
}