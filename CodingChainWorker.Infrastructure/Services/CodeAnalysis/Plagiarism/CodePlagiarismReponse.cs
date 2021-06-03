using System;
using System.Collections.Generic;
using CodingChainApi.Infrastructure.Models;

namespace CodingChainApi.Infrastructure.Services.CodeAnalysis.Plagiarism
{
    public class CodePlagiarismReponse
    {
        private List<PlagiarizedFunction> plagiarizedFunctions = new();

        public void addPlagiarizedFunction(Guid plagiarizedFunctionId, Guid functionComparedId, double rate)
        {
            plagiarizedFunctions.Add(new PlagiarizedFunction(plagiarizedFunctionId, functionComparedId, rate));
        }
    }
}