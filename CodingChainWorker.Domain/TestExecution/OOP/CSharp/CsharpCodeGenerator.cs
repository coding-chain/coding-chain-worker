using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Exceptions;

namespace Domain.TestExecution.OOP.CSharp
{
    public class CsharpCodeGenerator : IPooCodeGenerator
    {
        private List<OoTest> _tests;
        private SortedSet<OoFunction> _functions;
        private IPooCodeAnalyzer _analyzer;
        private string _headerCode;


        public CsharpCodeGenerator(IList<Test> tests, IList<Function> functions, IPooCodeAnalyzer analyzer,
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

        private OoFunction ToOoFunction(Function func)
        {
            var className = GetClassName("Function", func.Id);
            var funcName = _analyzer.FindMethodName(func.CleanedCode) ??
                           throw new DomainException($"Cannot find method name for function {func}");
            return new OoFunction(ToStaticClass(func.CleanedCode, className), func.Id, funcName, className);
        }

        private OoTest ToOoTest(Test test, int order)
        {
            var outClassName = GetClassName("OutputValidator", order);
            var inClassName = GetClassName("InputGenerator", order);
            var inMethodName = _analyzer.FindMethodName(test.InputGenerator) 
                               ?? throw new DomainException($"Cannot find method name for input generator on test {test}") ;
            var outMethodName = _analyzer.FindMethodName(test.OutputValidator)
                                ?? throw new DomainException($"Cannot find method name for output validator on test {test}") ;
            var inFunc = new OoFunction(ToStaticClass(test.CleanedInputGenerator, inClassName), order,
                inMethodName, inClassName);
            var outFunc = new OoFunction(ToStaticClass(test.CleanedOutputValidator, outClassName), order,
                outMethodName, outClassName);
            return new OoTest(inFunc, outFunc);
        }

        public string GetExecutableCode()
        {
            var builder = new StringBuilder();
            builder.AppendLine(_headerCode);
            builder.AppendLine(CreateFunctionDeclarations());
            builder.AppendLine(CreateTestClass());
            var res = builder.ToString();
            return res;
        }

        public string CreateTestFunction(OoTest test, int order)
        {
            var builder = new StringBuilder();
            builder.AppendLine($@"
            [Test]
            public void Test{order}(){{
                Assert.True({CreatePipeline(test)});
            }}");
            return builder.ToString();
        }

        public string CreatePipeline(OoTest test)
        {
            var builder = new StringBuilder();
            var allFunctions = new List<OoFunction>() {test.InFunc};
            allFunctions.AddRange(_functions);
            allFunctions.Add(test.OutFunc);
            builder.Append(GetFunctionCall(new Stack<OoFunction>(allFunctions)));

            return builder.ToString();
        }

        private string GetFunctionCall(Stack<OoFunction> functionsStack)
        {
            if (functionsStack.Count == 0) return "";
            var function = functionsStack.Pop();
            return function.MethodCall(GetFunctionCall(functionsStack));
        }

        public string CreateTestClass()
        {
            var builder = new StringBuilder();
            builder.AppendLine("public class Tests {");
            for (var i = 0; i < _tests.Count; i++)
            {
                builder.AppendLine(CreateTestFunction(_tests[i], i));
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