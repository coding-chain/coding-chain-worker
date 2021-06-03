using System;

namespace CodingChainApi.Infrastructure.Models
{
    public class PlagiarizedFunction
    {
        private Guid _plagiarizeFunctionId;
        private Guid _functionCompared;
        private double _rate;

        public PlagiarizedFunction(Guid functionCompared, Guid plagiarizeFunctionId, double rate)
        {
            _functionCompared = functionCompared;
            _plagiarizeFunctionId = plagiarizeFunctionId;
            _rate = rate;
        }
    }
}