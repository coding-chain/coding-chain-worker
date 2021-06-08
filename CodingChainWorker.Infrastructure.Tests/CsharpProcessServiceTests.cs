//
//
// using System;
// using System.Collections.Generic;
// using CodingChainApi.Infrastructure.Services.Processes;
// using CodingChainApi.Infrastructure.Settings;
// using Domain.TestExecution;
// using Domain.TestExecution.OOP.CSharp;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using NUnit.Framework;
//
// namespace CodingChainWorker.Infrastructure.Tests
// {
//     public class FakeLogger : Logger<CsharpProcessService>
//     {
//         public FakeLogger(ILoggerFactory factory) : base(factory)
//         {
//         }
//     }
//
//     public class FakeParticipationTestingAggregate : CSharpParticipationTestingAggregate
//     {
//         public string ExecutionCode { get; set; }
//
//
//         public override string GetExecutableCode() => ExecutionCode;
//
//         public FakeParticipationTestingAggregate(ParticipationId id, string language, string headerCode,
//             IList<FunctionEntity> functions, IList<TestEntity> tests) : base(id, language, headerCode, functions, tests)
//         {
//         }
//     }
//
//     public class CsharpProcessServiceTests
//     {
//         private CsharpProcessService _processService;
//         private AppDataSettings _appDataSettings;
//         private CSharpExecutionSettings _cSharpExecutionSettings;
//         private CSharpParticipationTestingAggregate _participationTestingAggregate;
//
//         [SetUp]
//         public void Setup()
//         {
//             _appDataSettings = new AppDataSettings()
//             {
//                 BasePath = "../../../AppData",
//                 TemplatesPath = "Templates"
//             };
//             _cSharpExecutionSettings = new CSharpExecutionSettings()
//             {
//                 TemplatePath = "csharp_template",
//                 BaseTestFileName = "Tests"
//             };
//             _participationTestingAggregate = new FakeParticipationTestingAggregate(
//                 new ParticipationId(Guid.NewGuid()),
//                 "csharp",
//                 "import System;",
//                 new List<FunctionEntity> {new("public static string test1(string test) { return test; }", 0)},
//                 new List<TestEntity>
//                 {
//                     new("public static bool test1(string test) { return \"test\"==test; }",
//                         "public static string test1() { return \"test\"; }")
//                 });
//             var serviceProvider = new ServiceCollection()
//                 .AddLogging()
//                 .BuildServiceProvider();
//
//             var factory = serviceProvider.GetService<ILoggerFactory>();
//             _processService =
//                 new CsharpProcessService(_appDataSettings, _cSharpExecutionSettings, new FakeLogger(factory));
//         }
//
//         [Test]
//         public void Test1()
//         {
//             var handler = _processService.WriteAndExecuteParticipation(_participationTestingAggregate);
//             // handler.ProcessEnded += (sender, args) => Console.WriteLine(args);
//             // Assert.Pass();
//         }
//     }
// }

