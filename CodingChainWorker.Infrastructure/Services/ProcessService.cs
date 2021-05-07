using System;
using System.IO;
using Application.Contracts.IService;
using Domain.TestExecution;

namespace CodingChainApi.Infrastructure.Services
{
    public abstract class ProcessService<T> : IProcessService<T>, IDisposable where T : ParticipationTestingAggregate
    {
        
        abstract protected string TemplatePath { get;  }
        protected abstract string TestsFilePath { get; }
        private StreamWriter? _fileStream;
        protected StreamWriter FileStream  {
            get
            {
                _fileStream ??= new StreamWriter(TestsFilePath, false);
                return _fileStream;
            }
        }

        protected ProcessService()
        {
            
        }
        public void Dispose()
        {
            _fileStream?.Dispose();
        }
        public abstract void ExecuteParticipationCode(T participation);

        public string  WriteParticipation(T participation)
        {
            FileStream.WriteLine(participation.GetExecutableCode());
            FileStream.Close();
            _fileStream = null;
            return TestsFilePath;
        }
    }
}