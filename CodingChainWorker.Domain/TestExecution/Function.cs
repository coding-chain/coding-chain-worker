using System;
using System.Text.RegularExpressions;

namespace Domain.TestExecution
{
    public record Function: IComparable<Function>
    {
        public Function(string code, int id)
        {
            Code = code;
            Id = id;
            CleanedCode = Regex.Replace(code, @"\s+", " ");
        }

        public string CleanedCode { get; init; }
        public string Code { get; }
        public int Id { get; }

        public void Deconstruct(out string cleanedCode, out string code, out int order)
        {
            cleanedCode = CleanedCode;
            code = Code;
            order = Id;
        }

        public int CompareTo(Function? other)
        {
            return Id.CompareTo(other?.Id);
        }

        public override string ToString()
        {
            return $"{nameof(CleanedCode)}: {CleanedCode}, {nameof(Code)}: {Code}, {nameof(Id)}: {Id}";
        }
    }
}