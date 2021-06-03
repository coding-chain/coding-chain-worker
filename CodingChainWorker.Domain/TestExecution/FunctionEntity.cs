using System;
using System.Text.RegularExpressions;
using Domain.Contracts;

namespace Domain.TestExecution
{
    public record FunctionId(Guid Value) : IEntityId
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class FunctionEntity : Entity<FunctionId>, IComparable<FunctionEntity>
    {
        public FunctionEntity(string code,int order, FunctionId id) : base(id)
        {
            Order = order;
            Code = code;
            CleanedCode = Regex.Replace(code, @"\s+", " ");
        }

        public string CleanedCode { get; init; }
        public string Code { get; }
        public int Order { get; }


        public int CompareTo(FunctionEntity? other)
        {
            return Order.CompareTo(other?.Order);
        }

        public override string ToString()
        {
            return $"{nameof(CleanedCode)}: {CleanedCode}, {nameof(Code)}: {Code}, {nameof(Id)}: {Id}";
        }
    }
}