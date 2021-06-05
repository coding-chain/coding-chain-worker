using System.Collections.Generic;
using CodingChainApi.Infrastructure.Models;

namespace CodingChainApi.Infrastructure.Services.CodeAnalysis.Plagiarism
{
    public interface ICodePlagiarismService
    {
        public List<PlagiarizedFunction> AnalyseCode(Function suspectedFunction, IList<Function> functionsToCompare);
    }
}