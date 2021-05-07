using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Application.Contracts.IService;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution;
using Domain.TestExecution.POO;

namespace CodingChainApi.Infrastructure.Services
{
    public class CsharpProcessService : ProcessService<CSharpParticipationTestingAggregate>
    {
        private readonly IAppDataSettings _appDataSettings;
        private readonly ICSharpExecutionSettings _cSharpExecutionSettings;

        protected override string TemplatePath => Path.Combine(_appDataSettings.BasePath, _appDataSettings.TemplatesPath,
            _cSharpExecutionSettings.TemplatePath);

        protected override string TestsFilePath => Path.Combine(TemplatePath, $"{_cSharpExecutionSettings.BaseTestFileName}.cs");
        private const string ProcessName = "dotnet";
        private const string TestCommand = "test";
        
        public CsharpProcessService(IAppDataSettings appDataSettings, ICSharpExecutionSettings cSharpExecutionSettings)
        {
            _appDataSettings = appDataSettings;
            _cSharpExecutionSettings = cSharpExecutionSettings;
        }

        public override void ExecuteParticipationCode(CSharpParticipationTestingAggregate participation)
        {
            // using var process = new Process
            // {
            //     StartInfo =
            //     {
            //         FileName = ProcessName,
            //         RedirectStandardInput = true,
            //         RedirectStandardOutput = true,
            //         CreateNoWindow = false,
            //         UseShellExecute = false,
            //         Arguments = $"{TestCommand} {TemplatePath}"
            //     }
            // };
            //
            // process.ErrorDataReceived += (o,e)=> Console.WriteLine(e.Data);
            // process.OutputDataReceived += (o,e)=> Console.WriteLine(e.Data);
            // process.Start();
            // process.BeginOutputReadLine();
            // process.WaitForExit();
        }
    }
}