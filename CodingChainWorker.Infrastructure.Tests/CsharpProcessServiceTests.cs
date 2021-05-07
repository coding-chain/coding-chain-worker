// using System;
// using System.Collections.Generic;
// using CodingChainApi.Infrastructure.Services;
// using CodingChainApi.Infrastructure.Settings;
// using Domain.TestExecution;
// using NUnit.Framework;
//
// namespace CodingChainWorker.Infrastructure.Tests
// {
//
//     public class CsharpProcessServiceTests
//     {
//         private CsharpProcessService _codeWriterService;
//         private AppDataSettings _appDataSettings;
//         private CSharpExecutionSettings _cSharpExecutionSettings;
//         private ParticipationTestingAggregate _participationTestingAggregate;
//
//         private string GetTestFunctionCode(int order) => $@"    
//     public static class FunctionClass{order}
//     {{
//         public static string func1(string test)
//         {{
//             return test;
//         }}
//     }}";
//
//         private string GetTestHeaderCode() => @"
//     using System;
// ";
//
//         [SetUp]
//         public void Setup()
//         {
//             _appDataSettings = new AppDataSettings()
//             {
//                 BasePath = "",
//                 TemplatesPath = ""
//             };
//             _cSharpExecutionSettings = new CSharpExecutionSettings()
//             {
//                 TemplatePath = "",
//                 BaseTestFileName = "Tests"
//             };
//             _participationTestingAggregate = new CSharpParticipationTestingAggregate(
//                 new ParticipationId(Guid.NewGuid()),
//                 "csharp",
//                 GetTestHeaderCode(),
//                 new List<ParticipationTestingAggregate.Function>()
//                 {
//                     new(GetTestFunctionCode(1), 1)
//                 },
//                 new List<ParticipationTestingAggregate.Test>()
//                 {
//                     new("", "")
//                 }
//             );
//             _codeWriterService = new CsharpProcessService(_appDataSettings, _cSharpExecutionSettings);
//         }
//
//         [Test]
//         public void Test1()
//         {
//             _codeWriterService.WriteParticipation(_participationTestingAggregate);
//             Assert.Pass();
//         }
//     }
// }