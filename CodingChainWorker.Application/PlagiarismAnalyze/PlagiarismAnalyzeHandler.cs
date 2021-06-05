using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Plagiarism;
using Domain.Plagiarism.Models;
using Domain.TestExecution;
using MediatR;

namespace Application.PlagiarismAnalyze
{
    public record Function(Guid Id, string Code);

    public record PlagiarismAnalyzeCommand
        (Function SuspectedFunction, IList<Function> ComparedFunctions) : INotification;

    public class PlagiarismAnalyzeHandler : INotificationHandler<PlagiarismAnalyzeCommand>
    {
        private readonly IPlagiarismSettings _plagiarismSettings;
        private readonly ICodePlagiarismService _codePlagiarismService;

        public PlagiarismAnalyzeHandler(IPlagiarismSettings plagiarismSettings,
            ICodePlagiarismService codePlagiarismService)
        {
            _plagiarismSettings = plagiarismSettings;
            _codePlagiarismService = codePlagiarismService;
        }

        public Task Handle(PlagiarismAnalyzeCommand notification, CancellationToken cancellationToken)
        {
            var suspectedFunction = new FunctionAggregate(new FunctionId(notification.SuspectedFunction.Id),
                notification.SuspectedFunction.Code);
            var comparedFunctions = notification.ComparedFunctions
                .Select(f => new FunctionAggregate(new FunctionId(f.Id), f.Code))
                .ToList();
            _codePlagiarismService.AnalyseCode(suspectedFunction, comparedFunctions, _plagiarismSettings);
            return Task.CompletedTask;
        }
    }
}