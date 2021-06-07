using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Domain.Plagiarism;
using Domain.Plagiarism.Models;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.PlagiarismAnalyze
{
    public record Function(Guid Id, string Code);

    public record FunctionSimilarity(Guid Id, double Rate);

    public record PlagiarismAnalyzeNotification
        (Function SuspectedFunction, IList<Function> ComparedFunctions) : INotification;

    public record PlagiarismAnalyzeResponse
        (Guid SuspectedFunctionId, IList<FunctionSimilarity> ComparedFunctions);

    public class PlagiarismAnalyzeHandler : INotificationHandler<PlagiarismAnalyzeNotification>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPlagiarismSettings _plagiarismSettings;

        public PlagiarismAnalyzeHandler(IServiceProvider serviceProvider, IPlagiarismSettings plagiarismSettings)
        {
            _plagiarismSettings = plagiarismSettings;
            _serviceProvider = serviceProvider;
        }

        public Task Handle(PlagiarismAnalyzeNotification notification, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var codePlagiarismService = scope.ServiceProvider
                .GetRequiredService<ICodePlagiarismService>();
            var donePlagiarismService = scope.ServiceProvider
                .GetRequiredService<IDispatcher<PlagiarismAnalyzeResponse>>();


            var suspectedFunction = new FunctionAggregate(new FunctionId(notification.SuspectedFunction.Id),
                notification.SuspectedFunction.Code);
            var comparedFunctions = notification.ComparedFunctions
                .Select(f => new FunctionAggregate(new FunctionId(f.Id), f.Code))
                .ToList();
            var functionWithSimilarities =
                codePlagiarismService.AnalyseCode(suspectedFunction, comparedFunctions, _plagiarismSettings);
            var similarFunctions = functionWithSimilarities.SimilarFunctions
                .Select(f => new FunctionSimilarity(f.Id.Value, f.SimilarityRate))
                .ToList();
            var res = new PlagiarismAnalyzeResponse(functionWithSimilarities.Id.Value, similarFunctions);
            donePlagiarismService.Dispatch(res);
            return Task.CompletedTask;
        }
    }
}