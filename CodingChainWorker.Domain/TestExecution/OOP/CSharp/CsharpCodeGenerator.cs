using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Exceptions;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class CsharpCodeGenerator : CodeGenerator<OoFunction>
    {
        public override string GetTestNameByOrder(int order) => $"{TestPrefix}{order}";
        public override string TestPrefix => "Test";
        private ICodeAnalyzer _analyzer;
        private string _headerCode;
        protected override string CustomHeader => new StringBuilder("using NUnit.Framework;")
            .Append(_headerCode)
            .ToString();

        public CsharpCodeGenerator(IList<TestEntity> tests, IList<FunctionEntity> functions, ICodeAnalyzer analyzer,
            string? headerCode)
        {
            _analyzer = analyzer;
            _headerCode = headerCode ?? "";
            SortedFunctions = new SortedSet<OoFunction>(functions.Select(ToOoFunction));
            TestsList = tests.Select(ToOoTest).ToList();
        }

        private string ToStaticClass(string code, string className)
        {
            return $@"
             public static class {className} {{
                 {code}
             }}";
        }

        private string GetClassName(string name, int order) => $"{name}{order}";

        private OoFunction ToOoFunction(FunctionEntity func)
        {
            var className = GetClassName("Function", func.Order);
            var funcName = _analyzer.FindFunctionName(func.Code) ??
                           throw new DomainException($"Cannot find method name for function {func}");
            return new OoFunction(ToStaticClass(func.Code, className), func.Order, funcName, className, func.Id);
        }

        private Test<OoFunction> ToOoTest(TestEntity test, int order)
        {
            var outClassName = GetClassName("OutputValidator", order);
            var inClassName = GetClassName("InputGenerator", order);
            var inMethodName = _analyzer.FindFunctionName(test.InputGenerator)
                               ?? throw new DomainException(
                                   $"Cannot find method name for input generator on test {test}");
            var outMethodName = _analyzer.FindFunctionName(test.OutputValidator)
                                ?? throw new DomainException(
                                    $"Cannot find method name for output validator on test {test}");
            var inFunc = new OoFunction(ToStaticClass(test.InputGenerator, inClassName), order,
                inMethodName, inClassName);
            var outFunc = new OoFunction(ToStaticClass(test.OutputValidator, outClassName), order,
                outMethodName, outClassName);
            return new Test<OoFunction>(inFunc, outFunc, test.Id, GetTestNameByOrder(order));
        }


        protected override string GetFormattedTestsBlock(string content)
        {
            return @$"public class Tests {{
            {content}
              }}";
        }

        protected override string GetFormattedTestContent(Test<OoFunction> test, string testContent)
        {
            return @$"[Test]
            public void {test.Name}(){{
                Assert.True({testContent});
            }}";
        }

    }
}