using System;
using System.Collections.Generic;
using CodingChainApi.Infrastructure.Services;
using CodingChainApi.Infrastructure.Services.Processes;
using CodingChainApi.Infrastructure.Settings;
using Domain.Contracts;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Test = Domain.TestExecution.Test;

namespace CodingChainWorker.Infrastructure.Tests
{
    public class FakeLogger : Logger<CsharpProcessService>
    {
        public FakeLogger(ILoggerFactory factory) : base(factory)
        {
        }
    }
    
    public class FakeParticipationTestingAggregate : CSharpParticipationTestingAggregate
    {
        public string ExecutionCode { get; set; }


        public override string GetExecutableCode() => ExecutionCode;

        public FakeParticipationTestingAggregate(ParticipationId id, string language, string headerCode,
            IList<Function> functions, IList<Test> tests) : base(id, language, headerCode, functions, tests)
        {
        }
    }

    public class CsharpProcessServiceTests
    {
        private CsharpProcessService _processService;
        private AppDataSettings _appDataSettings;
        private CSharpExecutionSettings _cSharpExecutionSettings;
        private CSharpParticipationTestingAggregate _participationTestingAggregate;

        [SetUp]
        public void Setup()
        {
            _appDataSettings = new AppDataSettings()
            {
                BasePath = "",
                TemplatesPath = ""
            };
            _cSharpExecutionSettings = new CSharpExecutionSettings()
            {
                TemplatePath = "",
                BaseTestFileName = "Tests"
            };
            _participationTestingAggregate = new FakeParticipationTestingAggregate(
                new ParticipationId(Guid.NewGuid()),
                "csharp",
                "import System;",
                new List<Function>(),
                new List<Test>());
            _processService = new CsharpProcessService(_appDataSettings, _cSharpExecutionSettings, new FakeLogger(null));
        }

        [Test]
        public void Test1()
        {
            var handler = _processService.WriteAndExecuteParticipation(_participationTestingAggregate);
            handler.ProcessEnded += (sender, args) => Console.WriteLine(args);
            Assert.Pass();
        }
    }
}