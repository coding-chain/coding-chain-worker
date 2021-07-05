using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.TestExecution.Helpers
{
    public abstract class CodeGenerator<TFunction> : ICodeGenerator<TFunction> where TFunction : FunctionBase
    {
        protected SortedSet<TFunction> SortedFunctions { get; init; }
        protected List<Test<TFunction>> TestsList { get; init; }
        protected virtual string? CustomHeader { get; } = null;
        public IReadOnlyCollection<Test<TFunction>> Tests => TestsList.AsReadOnly();
        public IReadOnlyCollection<FunctionBase> Functions => SortedFunctions.ToList().AsReadOnly();

        public string GetExecutableCode()
        {
            var builder = new StringBuilder();
            builder.AppendLine(CustomHeader);
            builder.AppendLine(CreateFunctionDeclarations());
            builder.AppendLine(CreateTestsBlock());
            return builder.ToString();
        }

        public abstract string GetTestNameByOrder(int order);
        public abstract string TestPrefix { get; }

        protected string CreateFunctionDeclarations()
        {
            var builder = new StringBuilder();
            foreach (var sortedFunction in Functions) builder.Append(sortedFunction.Code);

            foreach (var tests in Tests)
            {
                builder.AppendLine(tests.InFunc.Code);
                builder.AppendLine(tests.OutFunc.Code);
            }

            return builder.ToString();
        }

        protected string CreatePipeline(Test<TFunction> test)
        {
            var builder = new StringBuilder();
            var allFunctions = new List<TFunction> {test.InFunc};
            allFunctions.AddRange(SortedFunctions);
            allFunctions.Add(test.OutFunc);
            builder.Append(GetFunctionCall(new Stack<TFunction>(allFunctions)));

            return builder.ToString();
        }

        protected string GetFunctionCall(Stack<TFunction> functionsStack)
        {
            if (functionsStack.Count == 0) return "";
            var function = functionsStack.Pop();
            return function.FunctionCall(GetFunctionCall(functionsStack));
        }

        private string CreateTestFunction(Test<TFunction> test)
        {
            var builder = new StringBuilder();
            builder.AppendLine(GetFormattedTestContent(test, CreatePipeline(test)));
            return builder.ToString();
        }

        private string CreateTestsBlock()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < TestsList.Count; i++) builder.AppendLine(CreateTestFunction(TestsList[i]));

            return new StringBuilder()
                .AppendLine(GetFormattedTestsBlock(builder.ToString()))
                .ToString();
        }

        protected abstract string GetFormattedTestsBlock(string content);

        protected abstract string GetFormattedTestContent(Test<TFunction> test, string testContent);
    }
}