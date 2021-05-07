using System;
using System;
using System.Collections.Generic;
using Domain.TestExecution;
using Domain.TestExecution.POO;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests
{
    public class CSharpParticipationTestingAggregateTests
    {
        private ParticipationTestingAggregate _participationTestingAggregate;

        private string GetTestFunctionCode(string name,int order, string type = null, string returnType = null , string code = "return test;" ) => $@"    
        public static {returnType ?? type} {name}{order}({(type == null ? "" : $"{type} test")})
        {{
            {code}
        }}";
        
        private string GetTestOutputGeneratorCode() => @"    
        public static  bool TestOutput(string test)
        {
            return ""salut"" == test ;
        }";
        
        private string GetTestInputGeneratorCode() => @"    
        public static string TestGen()
        {
            return ""salut"";
        }";

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
                    new(GetTestFunctionCode("test",2, "string"), 2),
                    new(GetTestFunctionCode("test",1, "string"), 1)
                },
                new List<Test>()
                {
                    new Test(
                        GetTestFunctionCode("test",1, "string", "bool", @"return ""test""==test;"),
                        GetTestFunctionCode("test",1, null, "string", @"return ""test"";")),
                    new Test(
                        GetTestFunctionCode("test",1, "string", "bool", @"return ""test""==test;"),
                        GetTestFunctionCode("test",1, null, "string", @"return ""test"";"))
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
