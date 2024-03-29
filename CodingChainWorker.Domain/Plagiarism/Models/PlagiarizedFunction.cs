using System;

namespace Domain.Plagiarism.Models
{
    public class PlagiarizedFunction
    {
        private Guid _functionCompared;
        private Guid _plagiarizeFunctionId;
        private string _rate;

        public PlagiarizedFunction(Guid functionCompared, Guid plagiarizeFunctionId, string rate)
        {
            _functionCompared = functionCompared;
            _plagiarizeFunctionId = plagiarizeFunctionId;
            _rate = rate;
        }
    }
}