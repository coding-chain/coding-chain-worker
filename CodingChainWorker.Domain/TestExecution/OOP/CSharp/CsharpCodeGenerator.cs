using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Exceptions;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class CsharpCodeGenerator : ICodeGenerator
    {
        public string GetTestNameByOrder(int order) => $"{TestPrefix}{order}";
        public IReadOnlyCollection<ITest> Tests => _tests.AsReadOnly();
        public IReadOnlyCollection<FunctionBase> Functions => _functions.ToList().AsReadOnly();
        public string TestPrefix => "Test";
        private List<OoTest> _tests;
        private SortedSet<OoFunction> _functions;
        private ICodeAnalyzer _analyzer;
        private string _headerCode;

        

        public CsharpCodeGenerator(IList<TestEntity> tests, IList<FunctionEntity> functions, ICodeAnalyzer analyzer,
            string headerCode)
        {
            _analyzer = analyzer;
            _headerCode = headerCode;
            _functions = new SortedSet<OoFunction>(functions.Select(ToOoFunction));
            _tests = tests.Select(ToOoTest).ToList();
        }

        public string ToStaticClass(string code, string className)
        {
            return $@"
             public static class {className} {{
                 {code}
             }}";
        }

        public string GetClassName(string name, int order) => $"{name}{order}";

        private OoFunction ToOoFunction(FunctionEntity func)
        {
            var className = GetClassName("Function", func.Order);
            var funcName = _analyzer.FindFunctionName(func.Code) ??
                           throw new DomainException($"Cannot find method name for function {func}");
            return new OoFunction(ToStaticClass(func.Code, className), func.Order, funcName, className, func.Id);
        }

        private OoTest ToOoTest(TestEntity test, int order)
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
            return new OoTest(inFunc, outFunc, test.Id, GetTestNameByOrder(order));
        }

        public string GetExecutableCode()
        {
            var builder = new StringBuilder();
            builder.AppendLine("using NUnit.Framework;");
            builder.AppendLine(_headerCode);
            builder.AppendLine(CreateFunctionDeclarations());
            builder.AppendLine(CreateTestClass());
            var res = builder.ToString();
            return res;
        }

        public string CreateTestFunction(OoTest test)
        {
            var builder = new StringBuilder();
            builder.AppendLine($@"
            [Test]
            public void {test.Name}(){{
                Assert.True({CreatePipeline(test)});
            }}");
            return builder.ToString();
        }

        public string CreatePipeline(OoTest test)
        {
            var builder = new StringBuilder();
            var allFunctions = new List<OoFunction>() {test.OoInFunc};
            allFunctions.AddRange(_functions);
            allFunctions.Add(test.OoOutFunc);
            builder.Append(GetFunctionCall(new Stack<OoFunction>(allFunctions)));

            return builder.ToString();
        }

        private string GetFunctionCall(Stack<OoFunction> functionsStack)
        {
            if (functionsStack.Count == 0) return "";
            var function = functionsStack.Pop();
            return function.FunctionCall(GetFunctionCall(functionsStack));
        }

        public string CreateTestClass()
        {
            var builder = new StringBuilder();
            builder.AppendLine("public class Tests {");
            for (var i = 0; i < _tests.Count; i++)
            {
                builder.AppendLine(CreateTestFunction(_tests[i]));
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        public string CreateFunctionDeclarations()
        {
            var builder = new StringBuilder();
            foreach (var sortedFunction in _functions)
            {
                builder.Append(sortedFunction.Code);
            }

            foreach (var ooTest in _tests)
            {
                builder.AppendLine(ooTest.InFunc.Code);
                builder.AppendLine(ooTest.OutFunc.Code);
            }

            return builder.ToString();
        }
    }
}