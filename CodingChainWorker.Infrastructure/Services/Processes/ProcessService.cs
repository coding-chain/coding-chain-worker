using System;
using System.IO;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Domain.TestExecution;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public abstract class ProcessService<T> : IProcessService<T>, IDisposable where T : ParticipationTestingAggregate
    {
        protected abstract string TestsFilePath { get; }
        private StreamWriter? _fileStream;
        protected StreamWriter FileStream  {
            get
            {
                _fileStream ??= new StreamWriter(TestsFilePath, false);
                return _fileStream;
            }
        }
        
        public void Dispose()
        {
            _fileStream?.Dispose();
        }

        public Task WriteAndExecuteParticipation(T participation)
        {
            WriteParticipation(participation);
            return ExecuteParticipation(participation);
        }
        public abstract Task ExecuteParticipation(T participation);

        public void WriteParticipation(T participation)
        {
            FileStream.WriteLine(participation.GetExecutableCode());
            _fileStream?.Close();
            _fileStream = null;
        }

    }
}