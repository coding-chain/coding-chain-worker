using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.TestExecution.POO
{
    public class CsharpCodeBuilder : IPooCodeBuilder
    {
        private List<OoTest> _tests;
        private SortedSet<OoFunction> _functions;
        private IPOOCodeAnalyzer _analyzer;
        private string _headerCode;


        public CsharpCodeBuilder(IList<Test> tests, IList<Function> functions, IPOOCodeAnalyzer analyzer,
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
            return new OoFunction(ToStaticClass(func.CleanedCode, className), func.Id,
                _analyzer.FindMethodName(func.CleanedCode), className);
        }

        private OoTest ToOoTest(Test test, int order)
        {
            var outputClassName = GetClassName("OutputValidator", order);
            var inputClassName = GetClassName("InputGenerator", order);
            var inFunc = new OoFunction(ToStaticClass(test.CleanedInputGenerator, inputClassName), order,
                _analyzer.FindMethodName(test.InputGenerator), inputClassName);
            var outFunc = new OoFunction(ToStaticClass(test.CleanedOutputValidator, outputClassName), order,
                _analyzer.FindMethodName(test.OutputValidator), outputClassName);
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