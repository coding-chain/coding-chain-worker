using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP
{
    public class Function: FunctionBase
    {
        public Function(string code, int order, string methodName, string className, FunctionId? id = null)
        {
            Code = code;
            Order = order;
            FunctionName = methodName;
            ClassName = className;
            Id = id;
        }

        public string ClassName { get; }
        public override string FunctionCall(string? parameters = null) => $"{ClassName}.{FunctionName}({parameters})";

    }
}