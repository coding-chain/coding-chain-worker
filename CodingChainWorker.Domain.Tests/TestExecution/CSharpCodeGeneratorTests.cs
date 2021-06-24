using System;
using System.Collections.Generic;
using CodingChainWorker.Domain.Tests.Helpers;
using Domain.Exceptions;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{
    public record Change(long Bill5, long Bill10, long Coin2);


    public class CSharpCodeGeneratorTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TestCase(6)]
        [Test]
        public void TestATM(int s)
        {
            var devicesTypes = new List<int>() {2, 5, 10};
            var devicesCnt = new List<int>() {0, 0, 0};
            var i = 2;
            Console.WriteLine(string.Format("{0,22:D8} {0,22:X8}", 1));
            while (i >= 0 && s > 0)
            {
                var testVal = s / devicesTypes[i];
                if (s % devicesTypes[i] == 0)
                {
                    s -= devicesTypes[i];
                    devicesCnt[i]++;
                }
                else
                {
                    i--;
                }
            }

            Console.WriteLine(devicesCnt);
        }


        [Test]
        public void constructor_should_throw_on_function_without_method_name()
        {
            var invalidFunctions = new List<FunctionEntity>
            {
                new(FunctionsTestHelper.GetTestFunctionCode(null, null), 1,
                    new FunctionId(Guid.NewGuid()))
            };
            Assert.Throws<DomainException>(() =>
                new CsharpCodeGenerator(new List<TestEntity>(), invalidFunctions, new CsharpCodeAnalyzer(), ""));
        }

        [Test]
        public void constructor_should_throw_on_test_without_input_method_name()
        {
            var inGen = FunctionsTestHelper.GetTestFunctionCode(null, null);
            var outVal = FunctionsTestHelper.GetTestFunctionCode("outputValidator", null, "string", "bool",
                @"return test == ""test"";");

            var invalidInputTest = new List<TestEntity>
            {
                new(new TestId(Guid.NewGuid()), outVal, inGen)
            };
            Assert.Throws<DomainException>(() =>
                new CsharpCodeGenerator(invalidInputTest, new List<FunctionEntity>(), new CsharpCodeAnalyzer(), ""));
        }

        [Test]
        public void constructor_should_throw_on_test_without_output_method_name()
        {
            var outVal = FunctionsTestHelper.GetTestFunctionCode(null, null, "string", "bool",
                @"return test == ""test"";");
            var inGen =
                FunctionsTestHelper.GetTestFunctionCode("inputGenerator", null, null, "string", @"return  ""input"";");

            var invalidInputTest = new List<TestEntity>
            {
                new(new TestId(Guid.NewGuid()), outVal, inGen)
            };
            Assert.Throws<DomainException>(() =>
                new CsharpCodeGenerator(invalidInputTest, new List<FunctionEntity>(), new CsharpCodeAnalyzer(), ""));
        }
    }
}