using System;

namespace Application.Contracts.Processes
{
    public interface IProcessEndHandler
    {
        public string Output { get; }
        public string Error { get; }
        public event EventHandler<ProcessEndedEventArgs> ProcessEnded;
    }
}