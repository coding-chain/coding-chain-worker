using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Application.Contracts.IService;
using CodingChainApi.Infrastructure.Common.Extensions;
using Domain.TestExecution;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Services.Processes
{
    public abstract class ProcessService : IProcessService, IDisposable
    {
        protected abstract FileInfo TestsFilePath { get; }
        private StreamWriter? _fileStream;
        private readonly IDirectoryService _directoryService;
        protected FileInfo? TemplateDirectoryPath { get; set; }
        protected abstract string ProcessArguments { get; }
        protected abstract string ProcessName { get; }
        protected abstract ILogger<ProcessService> Logger { get; set; }

        protected ProcessService(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        protected StreamWriter FileStream
        {
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

        public async Task WriteAndExecuteParticipation(ParticipationAggregate participation)
        {
            WriteParticipation(participation);
            await ExecuteParticipation(participation);
        }

        public Task PrepareParticipationExecution(ParticipationAggregate participation)
        {
            return _directoryService.GetTemplateDirectoryByParticipation(participation).ToTask();
        }

        public Task CleanParticipationExecution(ParticipationAggregate participation)
        {
            _directoryService.DeleteParticipationDirectory(participation);
            return Task.CompletedTask;
        }


        protected async Task ExecuteParticipation(ParticipationAggregate participation)
        {
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = ProcessName,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    Arguments = ProcessArguments
                },
                EnableRaisingEvents = true
            };
            process.ErrorDataReceived += (o, e) =>
            {
                participation.AddError(e.Data ?? "");
                Logger.LogDebug("{Error}", e.Data);
            };
            process.OutputDataReceived += (o, e) =>
            {
                participation.AddOutput(e.Data ?? "");
                Logger.LogDebug("{Output}", e.Data);
            };
            process.Exited += (o, e) => { Logger.LogDebug("Process ended"); };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();
            process.WaitForExit();
        }

        public void WriteParticipation(ParticipationAggregate participation)
        {
            TemplateDirectoryPath = _directoryService.GetTemplateDirectoryByParticipation(participation);
            FileStream.WriteLine(participation.GetExecutableCode());
            _fileStream?.Close();
            _fileStream = null;
        }
    }
}