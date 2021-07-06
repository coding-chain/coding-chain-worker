using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Domain.Contracts;
using Domain.Plagiarism;
using Domain.Plagiarism.Models;
using Domain.TestExecution;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.PlagiarismAnalyze
{
    public record Function(Guid Id, string Code);

    public record FunctionSimilarity(Guid Id, double Rate, string Hash);

    public record PlagiarismAnalyzeNotification
        (Function SuspectedFunction, IList<Function> ComparedFunctions) : INotification;

    public record PlagiarismAnalyzeResponse
        (Guid SuspectedFunctionId, string SuspectHash, IList<FunctionSimilarity> ComparedFunctions);

    public class PlagiarismAnalyzeHandler : INotificationHandler<PlagiarismAnalyzeNotification>
    {
        private readonly IPlagiarismSettings _plagiarismSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHashService _hashService;

        public PlagiarismAnalyzeHandler(IServiceProvider serviceProvider, IPlagiarismSettings plagiarismSettings,
            IHashService hashService)
        {
            _plagiarismSettings = plagiarismSettings;
            _hashService = hashService;
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
                notification.SuspectedFunction.Code, _hashService.GetHash(notification.SuspectedFunction.Code));
            var comparedFunctions = notification.ComparedFunctions
                .Select(f => new FunctionAggregate(new FunctionId(f.Id), f.Code, _hashService.GetHash(f.Code)))
                .ToList();
            var functionWithSimilarities =
                codePlagiarismService.AnalyseCode(suspectedFunction, comparedFunctions);
            var similarFunctions = functionWithSimilarities.SimilarFunctions
                .Select(f => new FunctionSimilarity(f.Id.Value, f.SimilarityRate, f.Hash))
                .ToList();
            var res = new PlagiarismAnalyzeResponse(functionWithSimilarities.Id.Value, functionWithSimilarities.Hash,
                similarFunctions);
            return donePlagiarismService.Dispatch(res);
        }
    }
}