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
        private readonly IDirectoryService _directoryService;
        private StreamWriter? _fileStream;

        protected ProcessService(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        protected abstract FileInfo TestsFilePath { get; }
        protected FileInfo? TemplateDirectoryPath { get; set; }
        protected abstract string ProcessArguments { get; }
        protected abstract string ProcessName { get; }
        protected abstract ILogger<ProcessService> Logger { get; set; }

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

        public async Task PrepareParticipationExecution(ParticipationAggregate participation)
        {
            Logger.LogInformation(
                "Template preparation started for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
            await _directoryService.GetTemplateDirectoryByParticipation(participation).ToTask();
            Logger.LogInformation(
                "Template preparation ended for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
        }

        public Task CleanParticipationExecution(ParticipationAggregate participation)
        {
            Logger.LogInformation(
                "Template deletion started for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
            _directoryService.DeleteParticipationDirectory(participation);
            Logger.LogInformation(
                "Template deletion ended for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
            return Task.CompletedTask;
        }

        public void WriteParticipation(ParticipationAggregate participation)
        {
            Logger.LogInformation(
                "Tests writing started for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
            TemplateDirectoryPath = _directoryService.GetTemplateDirectoryByParticipation(participation);
            FileStream.WriteLine(participation.GetExecutableCode());
            _fileStream?.Close();
            _fileStream = null;
            Logger.LogInformation(
                "Tests writing ended for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
        }


        protected Task ExecuteParticipation(ParticipationAggregate participation)
        {
            Logger.LogInformation(
                "Tests execution started for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
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
            process.ErrorDataReceived += (o, e) => { participation.AddError(e.Data ?? ""); };
            process.OutputDataReceived += (o, e) => { participation.AddOutput(e.Data ?? ""); };
            process.Exited += (o, e) => { Logger.LogDebug("Process ended"); };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            Logger.LogInformation(
                "Tests execution ended for participation : {ParticipationId} on language : {Language} ",
                participation.Id, participation.Language);
            return Task.CompletedTask;
        }
    }
}