using System;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public class ProcessEndHandler 
    {
        private string? _error;
        private string? _output;
        public string? Output => _output ;
        public string? Error => _error;

        public string AddError(string? newError)
        {
            _error += newError;
            return _error;
        }

        public string AddOutput(string? newOutput)
        {
            _output += newOutput;
            return _output;
        }
    }
}