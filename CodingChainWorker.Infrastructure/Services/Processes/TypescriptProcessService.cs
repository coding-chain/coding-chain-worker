using System.IO;
using System.Threading.Tasks;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution.Imperative.Typescript;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class TypescriptProcessService : ProcessService
    {
        private readonly ITypescriptExecutionSettings _typescriptExecutionSettings;
        public TypescriptProcessService(IDirectoryService directoryService, 
            ILogger<TypescriptProcessService> logger, ITypescriptExecutionSettings typescriptExecutionSettings) : base(
            directoryService)
        {
            Logger = logger;
            _typescriptExecutionSettings = typescriptExecutionSettings;
        }
        protected override FileInfo TestsFilePath  =>
            new(Path.Combine(TemplateDirectoryPath.FullName,
                $"{_typescriptExecutionSettings.BaseTestFileName}.spec.ts"));
        protected override string ProcessArguments => $"/c npm --prefix {TemplateDirectoryPath} run {TestCommand} & exit";
        protected override string ProcessName => "cmd.exe";

        private const string TestCommand = "test";
        protected sealed override ILogger<ProcessService> Logger { get; set; }
    }
}