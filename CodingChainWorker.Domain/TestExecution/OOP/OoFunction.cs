namespace Domain.TestExecution.OOP
{
    public record OoFunction : Function
    {
        public OoFunction(string code, int id, string methodName, string className) : base(code, id)
        {
            MethodName = methodName;
            ClassName = className;
        }

        public string MethodName { get; }
        public string ClassName { get; }
        public string MethodCall(string? parameters = null) => $"{ClassName}.{MethodName}({parameters})";
    }
}