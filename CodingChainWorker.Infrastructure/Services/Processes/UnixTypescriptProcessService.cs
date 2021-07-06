using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CodingChainApi.Infrastructure.Services.RightElevator;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class UnixTypescriptProcessService : ProcessService, ITypescriptProcessService
    {
        private const string TestCommand = "test";
        private readonly ITypescriptExecutionSettings _typescriptExecutionSettings;
        private readonly IRightElevatorService _rightElevatorService;

        public UnixTypescriptProcessService(IDirectoryService directoryService,
            ILogger<UnixTypescriptProcessService> logger, ITypescriptExecutionSettings typescriptExecutionSettings,
            IRightElevatorService rightElevatorService) : base(
            directoryService)
        {
            Logger = logger;
            _typescriptExecutionSettings = typescriptExecutionSettings;
            _rightElevatorService = rightElevatorService;
        }

        protected override FileInfo TestsFilePath =>
            new(Path.Combine(TemplateDirectoryPath.FullName,
                $"{_typescriptExecutionSettings.BaseTestFileName}.spec.ts"));

        protected override string ProcessArguments =>
            $" --prefix {TemplateDirectoryPath} run {TestCommand}";

        protected override string ProcessName => "npm";
        protected sealed override ILogger<ProcessService> Logger { get; set; }
        protected override Task PreExecuteParticipation(ParticipationAggregate participation)
        {
            return _rightElevatorService.Elevate(TemplateDirectoryPath.FullName);
        }
      
    }
}