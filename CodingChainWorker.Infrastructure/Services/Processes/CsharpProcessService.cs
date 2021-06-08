using System.IO;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class CsharpProcessService : ProcessService
    {
        private const string TestCommand = "test -v n";
        private readonly ICSharpExecutionSettings _cSharpExecutionSettings;

        public CsharpProcessService(ICSharpExecutionSettings cSharpExecutionSettings,
            IDirectoryService directoryService, ILogger<CsharpProcessService> logger) : base(directoryService)
        {
            _cSharpExecutionSettings = cSharpExecutionSettings;
            Logger = logger;
        }

        protected override string ProcessArguments => $"{TestCommand} {TemplateDirectoryPath}";
        protected override string ProcessName => "dotnet ";
        protected sealed override ILogger<ProcessService> Logger { get; set; }


        protected override FileInfo TestsFilePath =>
            new(Path.Combine(TemplateDirectoryPath.FullName,
                $"{_cSharpExecutionSettings.BaseTestFileName}.cs"));
    }
}