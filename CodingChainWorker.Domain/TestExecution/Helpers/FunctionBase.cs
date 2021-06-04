using System;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution.Helpers
{
    public abstract class FunctionBase : IComparable<OoFunction>
    {
        public string Code { get; set; }
        public int Order { get; set; }
        public FunctionId? Id { get; set; }
        public string FunctionName { get; set; }

        public int CompareTo(OoFunction? other)
        {
            return Order.CompareTo(other?.Order);
        }

        public abstract string FunctionCall(string? parameters = null);
    }
}