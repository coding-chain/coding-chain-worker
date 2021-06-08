using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.Imperative
{
    public class ImperativeFunction : FunctionBase
    {
        public ImperativeFunction(string code, int order, string functionName, FunctionId? id = null)
        {
            Code = code;
            Order = order;
            FunctionName = functionName;
            Id = id;
        }

        public override string FunctionCall(string? parameters = null)
        {
            return $"{FunctionName}({parameters})";
        }
    }
}