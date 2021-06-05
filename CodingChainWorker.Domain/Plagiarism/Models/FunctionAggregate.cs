using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts;
using Domain.TestExecution;

namespace Domain.Plagiarism.Models
{
    public class SimilarFunction : Entity<FunctionId>
    {
        public SimilarFunction(FunctionId id, double rate) : base(id)
        {
            SimilarityRate = rate;
        }
        double SimilarityRate { get; set; }
    }

    public class FunctionAggregate : Aggregate<FunctionId>
    {
        public string Code;
        public IReadOnlyCollection<SimilarFunction> SimilarFunctions => _similarFunctionsIds.ToList().AsReadOnly();
        private readonly HashSet<SimilarFunction> _similarFunctionsIds = new();

        public FunctionAggregate(FunctionId id, string code) : base(id)
        {
            Code = code;
        }

        public void AddSimilarFunction(FunctionId functionId, double rate)
        {
            _similarFunctionsIds.Add(new SimilarFunction(functionId, rate));
        }
    }
}