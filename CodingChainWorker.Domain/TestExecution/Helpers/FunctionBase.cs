using System;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution.Helpers
{
    public abstract class FunctionBase : IComparable<Function>
    {
        public string Code { get; set; }
        public int Order { get; set; }
        public FunctionId? Id { get; set; }
        public string FunctionName { get; set; }

        public int CompareTo(Function? other)
        {
            return Order.CompareTo(other?.Order);
        }

        public abstract string FunctionCall(string? parameters = null);
    }
}