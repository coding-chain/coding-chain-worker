namespace CodingChainWorker.Domain.Tests.Helpers
{
    public static class FunctionsTestHelper
    {
        public static string GetTestFunctionCode(string name, int? order, string type = null, string returnType = null,
            string code = "return test;")
        {
            return $@"    
        public static {returnType ?? type} {name}{order}({(type == null ? "" : $"{type} test")})
        {{
            {code}
        }}";
        }
    }
}