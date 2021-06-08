using System;
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
        public FunctionEntity(string code, int order, FunctionId id) : base(id)
        {
            Order = order;
            Code = code;
        }

        public string Code { get; }
        public int Order { get; }


        public int CompareTo(FunctionEntity? other)
        {
            return Order.CompareTo(other?.Order);
        }
    }
}