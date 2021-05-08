using System.Collections.Generic;
using CodingChainWorker.Domain.Tests.Helpers;
using Domain.Exceptions;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{
    public class CSharpCodeGeneratorTests
    {

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void constructor_should_throw_on_function_without_method_name()
        {
            var invalidFunctions = new List<Function>()
            {
                new Function(FunctionsTestHelper.GetTestFunctionCode(null, null), 1)
            };
            Assert.Throws<DomainException>(() =>
                new CsharpCodeGenerator(new List<Test>(), invalidFunctions, new CsharpCodeAnalyzer(), ""));
        }

        [Test]
        public void constructor_should_throw_on_test_without_input_method_name()
        {
            var inGen = FunctionsTestHelper.GetTestFunctionCode(null, null);
            var outVal = FunctionsTestHelper.GetTestFunctionCode("outputValidator", null, "string", "bool",
                @"return test == ""test"";");

            var invalidInputTest = new List<Test>()
            {
                new Test(outVal, inGen)
            };
            Assert.Throws<DomainException>(() =>
                new CsharpCodeGenerator(invalidInputTest, new List<Function>(), new CsharpCodeAnalyzer(), ""));
        }

        [Test]
        public void constructor_should_throw_on_test_without_output_method_name()
        {
            var outVal = FunctionsTestHelper.GetTestFunctionCode(null, null, "string", "bool",
                @"return test == ""test"";");
            var inGen =
                FunctionsTestHelper.GetTestFunctionCode("inputGenerator", null, null, "string", @"return  ""input"";");

            var invalidInputTest = new List<Test>()
            {
                new(outVal, inGen)
            };
            Assert.Throws<DomainException>(() =>
                new CsharpCodeGenerator(invalidInputTest, new List<Function>(), new CsharpCodeAnalyzer(), ""));
        }
    }
}