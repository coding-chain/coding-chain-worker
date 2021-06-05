using System;
using System.IO;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Domain.TestExecution;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public abstract class ProcessService<T> : IProcessService<T>, IDisposable where T : ParticipationTestingAggregate
    {
        protected abstract FileInfo TestsFilePath { get; }
        private StreamWriter? _fileStream;
        private readonly IDirectoryService _directoryService;
        protected FileInfo? TemplateDirectoryPath { get; set; }

        protected ProcessService(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        protected StreamWriter FileStream  {
            get
            {
                _fileStream ??= TestsFilePath.CreateText();
                return _fileStream;
            }
        }
        
        public void Dispose()
        {
            _fileStream?.Dispose();
        }

        public async Task WriteAndExecuteParticipation(T participation)
        {
            WriteParticipation(participation);
            await ExecuteParticipation(participation);
            _directoryService.DeleteParticipationDirectory(participation);
        }
        protected abstract Task ExecuteParticipation(T participation);

        public void WriteParticipation(T participation)
        {
            TemplateDirectoryPath = _directoryService.GetTemplateDirectoryByParticipation(participation);
            FileStream.WriteLine(participation.GetExecutableCode());
            _fileStream?.Close();
            _fileStream = null;
        }

    }
}