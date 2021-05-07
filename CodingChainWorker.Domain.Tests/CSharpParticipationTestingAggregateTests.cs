using System;
using System.Collections.Generic;
using Domain.TestExecution;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests
{
    public class CSharpParticipationTestingAggregateTests
    {
        private ParticipationTestingAggregate _participationTestingAggregate;

        private string GetTestFunctionCode(int order) => @"    
        public static string MyMethod(string test)
        {
            return test;
        }";
        
        private string GetTestOutputGeneratorCode() => @"    
        public static  bool TestOutput(string test)
        {
            return ""salut"" == test 
        }";
        
        private string GetTestInputGeneratorCode() => @"    
        public static string TestGen()
        {
            return test;
        }";

        private string GetTestHeaderCode() => @"
        using System;
        ";

        [SetUp]
        public void Setup()
        {
            _participationTestingAggregate = new CSharpParticipationTestingAggregate(
                new ParticipationId(Guid.NewGuid()),
                "csharp",
                GetTestHeaderCode(),
                new List<Function>()
                {
                    new(GetTestFunctionCode(1), 1)
                },
                new List<Test>()
                {
                    new(GetTestInputGeneratorCode(), GetTestOutputGeneratorCode())
                }
            );
        }

        [Test]
        public void Test1()
        {
            var res = _participationTestingAggregate.GetExecutableCode();
            Assert.Pass();
        }
        
    }
}