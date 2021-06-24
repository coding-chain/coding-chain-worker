using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Domain.Contracts;
using Domain.TestExecution;

namespace Domain.Plagiarism.Models
{
    public class SimilarFunction : Entity<FunctionId>
    {
        public SimilarFunction(FunctionId id, string hash, double rate) : base(id)
        {
            SimilarityRate = rate;
            Hash = hash;
        }

        public string Hash { get; set; }
        public double SimilarityRate { get; set; }
    }

    public class FunctionAggregate : Aggregate<FunctionId>
    {
        private readonly HashSet<SimilarFunction> _similarFunctions = new();
        public string Code { get; }

        public string Hash { get; }

        public FunctionAggregate(FunctionId id, string code, string hash) : base(id)
        {
            Code = Regex.Replace(code, @"\s+", " ");
            Hash = hash;
        }

        public IReadOnlyCollection<SimilarFunction> SimilarFunctions => _similarFunctions.ToList().AsReadOnly();

        public void AddSimilarFunction(FunctionId functionId, string hash, double rate)
        {
            _similarFunctions.Add(new SimilarFunction(functionId, hash, rate));
        }
    }
}