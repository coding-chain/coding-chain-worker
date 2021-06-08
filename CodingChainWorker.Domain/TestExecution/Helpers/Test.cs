namespace Domain.TestExecution.Helpers
{
    public class Test<TFunc> where TFunc : FunctionBase
    {
        public Test(TFunc inFunc, TFunc outFunc, TestId id, string name)
        {
            InFunc = inFunc;
            OutFunc = outFunc;
            Id = id;
            Name = name;
        }

        public TFunc InFunc { get; }
        public TFunc OutFunc { get; }

        public TestId Id { get; }

        public string Name { get; }
    }
}