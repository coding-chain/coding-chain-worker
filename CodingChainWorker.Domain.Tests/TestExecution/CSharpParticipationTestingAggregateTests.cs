using System;
using System;
using System.Collections.Generic;
using CodingChainWorker.Domain.Tests.Helpers;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests
{
    public class CSharpParticipationTestingAggregateTests
    {
        private ParticipationTestingAggregate _participationTestingAggregate;


        private string GetTestHeaderCode() => @"using System;";

        [SetUp]
        public void Setup()
        {
            _participationTestingAggregate = new CSharpParticipationTestingAggregate(
                new ParticipationId(Guid.NewGuid()),
                "csharp",
                GetTestHeaderCode(),
                new List<Function>()
                {
                    new(FunctionsTestHelper.GetTestFunctionCode("test",2, "string"), 2),
                    new(FunctionsTestHelper.GetTestFunctionCode("test",1, "string"), 1)
                },
                new List<Test>()
                {
                    new (
                        FunctionsTestHelper.GetTestFunctionCode("test",1, "string", "bool", @"return ""test""==test;"),
                        FunctionsTestHelper.GetTestFunctionCode("test",1, null, "string", @"return ""test"";")),
                    new (
                        FunctionsTestHelper.GetTestFunctionCode("test",1, "string", "bool", @"return ""test""==test;"),
                        FunctionsTestHelper.GetTestFunctionCode("test",1, null, "string", @"return ""test"";"))
                }
            );
        }


        [Test]
        public void get_executable_code_should_works()
        {
            var res = _participationTestingAggregate.GetExecutableCode();
            Assert.IsNotEmpty(res);
        }


    }
}
