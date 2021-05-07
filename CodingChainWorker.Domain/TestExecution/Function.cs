using System.Text.RegularExpressions;

namespace Domain.TestExecution
{
    public record Function
    {
        public Function(string code, int order)
        {
            Code = code;
            Order = order;
            CleanedCode = Regex.Replace(code, @"\s+", " ");
        }


        public string CleanedCode { get; init; }
        public string Code { get; }
        public int Order { get; }

        public void Deconstruct(out string cleanedCode, out string code, out int order)
        {
            cleanedCode = CleanedCode;
            code = Code;
            order = Order;
        }
    }
}