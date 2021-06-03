using System;

namespace CodingChainApi.Infrastructure.Models
{
    public class PlagiarizedFunction
    {
        private Guid _plagiarizeFunctionId;
        private Guid _functionCompared;
        private string _rate;

        public PlagiarizedFunction(Guid functionCompared, Guid plagiarizeFunctionId, string rate)
        {
            _functionCompared = functionCompared;
            _plagiarizeFunctionId = plagiarizeFunctionId;
            _rate = rate;
        }
    }
}