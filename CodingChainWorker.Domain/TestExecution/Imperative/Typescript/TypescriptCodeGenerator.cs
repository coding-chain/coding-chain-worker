using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.Imperative.Typescript
{
    public class TypescriptCodeGenerator : CodeGenerator<ImperativeFunction>
    {
        private readonly ICodeAnalyzer _analyzer;
        private readonly string _headerCode;

        public TypescriptCodeGenerator(IList<TestEntity> tests, IList<FunctionEntity> functions, ICodeAnalyzer analyzer,
            string? headerCode)
        {
            _analyzer = analyzer;
            _headerCode = headerCode ?? "";
            SortedFunctions = new SortedSet<ImperativeFunction>(functions.Select(ToImperativeFunction));
            TestsList = tests.Select(ToImperativeTest).ToList();
        }

        public override string TestPrefix => "test";

        private string GetFunctionName(string name, int order)
        {
            return $"{name}{order}";
        }

        private ImperativeFunction ToImperativeFunction(FunctionEntity function)
        {
            var funcName = _analyzer.FindFunctionName(function.Code) ??
                           throw new DomainException($"Cannot find function name for function {function.Id}");
            var newFuncName = GetFunctionName(TestPrefix, function.Order);
            var funcCode = function.Code.ReplaceFirst(funcName, newFuncName);
            return new ImperativeFunction(funcCode, function.Order, newFuncName, function.Id);
        }

        private Test<ImperativeFunction> ToImperativeTest(TestEntity test, int order)
        {
            var inFuncName = _analyzer.FindFunctionName(test.InputGenerator)
                             ?? throw new DomainException(
                                 $"Cannot find function name for input generator on test {test.Id}");
            var outFuncName = _analyzer.FindFunctionName(test.OutputValidator)
                              ?? throw new DomainException(
                                  $"Cannot find function name for output validator on test {test.Id}");
            var newInFuncName = GetFunctionName("inputGenerator", order);
            var newOutFuncName = GetFunctionName("outputValidator", order);
            var inCode = test.InputGenerator.ReplaceFirst(inFuncName, GetFunctionName("inputGenerator", order));
            var outCode = test.OutputValidator.ReplaceFirst(outFuncName, GetFunctionName("outputValidator", order));

            var inFunc = new ImperativeFunction(inCode, order, newInFuncName);
            var outFunc = new ImperativeFunction(outCode, order, newOutFuncName);
            return new Test<ImperativeFunction>(inFunc, outFunc, test.Id, GetTestNameByOrder(order));
        }

        protected override string GetFormattedTestsBlock(string content)
        {
            return @$"describe(""Participation tests"", () => {{
            {content}
            }});";
        }

        protected override string GetFormattedTestContent(Test<ImperativeFunction> test, string testContent)
        {
            return @$"test(""{test.Name}"", () => {{
                expect({testContent}).toBeTruthy()
            }});";
        }

        public override string GetTestNameByOrder(int order)
        {
            return $"test{order}";
        }
    }
}