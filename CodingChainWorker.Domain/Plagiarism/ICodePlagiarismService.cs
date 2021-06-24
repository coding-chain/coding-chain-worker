using System.Collections.Generic;
using Domain.Plagiarism.Models;

namespace Domain.Plagiarism
{
    public interface ICodePlagiarismService
    {
        public FunctionAggregate AnalyseCode(FunctionAggregate suspectedFunctionAggregate,
            IList<FunctionAggregate> functionsToCompare);
    }
}