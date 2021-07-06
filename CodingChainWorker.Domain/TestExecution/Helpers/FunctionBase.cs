using System;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution.Helpers
{
    public abstract class FunctionBase : IComparable<FunctionBase>
    {
        public string Code { get; set; }
        public int Order { get; set; }
        public FunctionId? Id { get; set; }
        public string FunctionName { get; set; }


        public abstract string FunctionCall(string? parameters = null);

        public int CompareTo(FunctionBase? other)
        {

            return Order.CompareTo(other?.Order);
        }
    }
}