using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.TestExecution
{
    public class CSharpParticipationTestingAggregate : ParticipationTestingAggregate
    {
        private const string InputVariableName = "input";

        public record CodeUsage(string MethodName, string ClassName)
        {
            public string MethodCall(string parameters) => $"{ClassName}.{MethodName}({parameters})";
        }

        public record CSharpFunction : Function
        {
            public CSharpFunction(string code, int order, CodeUsage codeUsage) : base(code, order)
            {
                CodeUsage = codeUsage;
            }

            public CodeUsage CodeUsage { get; }
        }

        public record CSharpTest : Test
        {
            public CSharpTest(string outputValidator, string inputGenerator, CodeUsage outputUsage,
                CodeUsage inputUsage, int order) : base(outputValidator, inputGenerator)
            {
                OutputUsage = outputUsage;
                InputUsage = inputUsage;
                Order = order;
            }

            public CodeUsage OutputUsage { get; }
            public CodeUsage InputUsage { get; }
            
            public int Order { get; }
        }


        private string ToStaticClass(string code, string className)
        {
            return $@"
             public static class {className} {{
                 {code}
             }}";
        }


        private static string FindMethodName(string code)
        {
            var match = Regex.Match(code, @"{[\s\S]*}|=>");
            var openBracketCnt = 0;
            var hasChanged = false;
            int i;
            for (i = match.Index; i >= 0; i--)
            {
                if (code[i] == ')')
                {
                    hasChanged = true;
                    openBracketCnt--;
                }

                if (code[i] == '(')
                {
                    hasChanged = true;
                    openBracketCnt++;
                }

                if (openBracketCnt == 0 && hasChanged) break;
            }

            code = code.Substring(0, i);
            return Regex.Match(code, @"\b(\w+)$").Value;
        }

        private SortedSet<CSharpFunction> _functions;
        private List<CSharpTest> _tests;
        public override IReadOnlyCollection<Function> Functions => _functions.ToList().AsReadOnly();
        public override IReadOnlyCollection<Test> Tests => _tests.ToList().AsReadOnly();
        private static string GetClassName(string name, int order) => $"{name}{order}";

        private CSharpFunction ToCSharpFunction(Function func)
        {
            var className = GetClassName("Function", func.Order);
            return new CSharpFunction(
                ToStaticClass(func.CleanedCode, className),
                func.Order,
                new CodeUsage(FindMethodName(func.CleanedCode), className));
        }

        private CSharpTest ToCSharpTest(Test test, int order)
        {
            var outputClassName = GetClassName("OutputValidator", order);
            var inputClassName = GetClassName("InputGenerator", order);
            var outputUsage = new CodeUsage(FindMethodName(test.OutputValidator), outputClassName);
            var inputUsage = new CodeUsage(FindMethodName(test.InputGenerator), inputClassName);
            return new CSharpTest(
                ToStaticClass(test.CleanedInputGenerator, outputClassName),
                ToStaticClass(test.InputGenerator, inputClassName),
                outputUsage,
                inputUsage,
                order);
        }

        public CSharpParticipationTestingAggregate(ParticipationId id, string language, string headerCode,
            IList<Function> functions, List<Test> tests) : base(id, language, headerCode)
        {
            _functions = new SortedSet<CSharpFunction>(functions.Select(ToCSharpFunction));
            _tests = tests.Select(ToCSharpTest).ToList();
        }


        public override string GetExecutableCode()
        {
            var builder = new StringBuilder();
            builder.Append(HeaderCode);

            builder.Append(CreateFunctionDeclarations());


            return builder.ToString();
        }

        private string GetAvailableTestClassName()
        {
            return "Testing1";
        }

        public string GetFunctionsPipeline(CSharpTest test)
        {
            var builder = new StringBuilder();
            foreach (var function in _functions)
            {
                
                var methodName = FindMethodName(function.CleanedCode);
            }

            return builder.ToString();
        }

        public string CreateTestFunction(CSharpTest test)
        {
            var builder = new StringBuilder();
            builder.Append(test.CleanedInputGenerator);
            builder.Append(test.CleanedOutputValidator);
            
        }

        public string CreateTestClass()
        {
            var builder = new StringBuilder();
            builder.Append($"public class {GetAvailableTestClassName()} {{");
            foreach (var test in _tests)
            {
                builder.Append(CreateTestFunction(test));
            }
            builder.Append("}");
            return builder.ToString();
        }

        public string CreateFunctionDeclarations()
        {
            var builder = new StringBuilder();
            foreach (var sortedFunction in _functions)
            {
                builder.Append(sortedFunction.Code);
            }
            return builder.ToString();
        }
    }
}