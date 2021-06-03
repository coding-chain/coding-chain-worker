using System;
using System.Collections.Generic;

namespace CodingChainApi.Infrastructure.Models
{
    public class Function
    {
        public Guid Id;
        public string Code;
        public int? Order;
        public bool IsDeleted;

        public Function(Guid id, string code, int? order, bool isDeleted)
        {
            Id = id;
            Code = code;
            Order = order;
            IsDeleted = isDeleted;
        }
    }
}