using System;
using System.Collections.Generic;
using CodingChainWorker.Domain.Tests.Helpers;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{
    public class CSharpParticipationTestingAggregateTests
    {
        private CSharpParticipationAggregate _participationAggregate;


        private string GetTestHeaderCode()
        {
            return @"using System;";
        }

        private TestId GetTestId()
        {
            return new(Guid.NewGuid());
        }

        private FunctionId GetFunctionId()
        {
            return new(Guid.NewGuid());
        }

        [SetUp]
        public void Setup()
        {
            // _participationAggregate = new CSharpParticipationAggregate(
            //     new ParticipationId(Guid.NewGuid()),
            //     LanguageEnum.CSharp,
            //     GetTestHeaderCode(),
            //     new List<FunctionEntity>
            //     {
            //         new(FunctionsTestHelper.GetTestFunctionCode("test", 2, "string"), 2, GetFunctionId()),
            //         new(FunctionsTestHelper.GetTestFunctionCode("test", 1, "string"), 1, GetFunctionId())
            //     },
            //     new List<TestEntity>
            //     {
            //         new(GetTestId(),
            //             FunctionsTestHelper.GetTestFunctionCode("test", 1, "string", "bool", @"return ""test""==test;"),
            //             FunctionsTestHelper.GetTestFunctionCode("test", 1, null, "string", @"return ""test"";")),
            //         new(GetTestId(),
            //             FunctionsTestHelper.GetTestFunctionCode("test", 1, "string", "bool", @"return ""test""==test;"),
            //             FunctionsTestHelper.GetTestFunctionCode("test", 1, null, "string", @"return ""test"";"))
            //     }
            // );
        }


        // [Test]
        // public void get_executable_code_should_works()
        // {
        //     var res = _participationAggregate.GetExecutableCode();
        //     Assert.IsNotEmpty(res);
        // }
    }
}