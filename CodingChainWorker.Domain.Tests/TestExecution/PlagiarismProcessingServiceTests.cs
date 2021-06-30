using System;
using System.Collections.Generic;
using CodingChainApi.Infrastructure.Services;
using CodingChainApi.Infrastructure.Settings;
using Domain.Contracts;
using Domain.Plagiarism;
using Domain.Plagiarism.Models;
using Domain.TestExecution;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{

    public class PlagiarismProcessingServiceTests
    {
        private List<FunctionAggregate> _functionsToCompare;
        private PlagiarismService _plagiarismService;
        private IPlagiarismSettings _settings;
        private FunctionAggregate _suspectedFunction;
        private HashService _hashService;

        [SetUp]
        public void Setup()
        {
            _settings = new PlagiarismSettings()
            {
                Threshold = 0.6,
                Configurations = new List<ComparisonConfig>
                {
                    new() { SamplingWindow = 6, KGramLength = 3 },
                    new() { SamplingWindow = 8, KGramLength = 5 }
                }
            };
            _hashService = new HashService();
            _plagiarismService = new PlagiarismService(_settings, _hashService);
            _functionsToCompare = new List<FunctionAggregate>(new[]
            {
                GetFunctionAggregate(
                    "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();var factory = serviceProvider.GetService<ILoggerFactory>();"
                )
            });
            _suspectedFunction = GetFunctionAggregate(
                "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();");
        }

        private FunctionAggregate GetFunctionAggregate(string code) =>
            new(new FunctionId(Guid.NewGuid()), code, _hashService.GetHash(code));


        [Test]
        public void ShouldInitTestRight()
        {
            var response = _plagiarismService.AnalyseCode(_suspectedFunction, _functionsToCompare);
            Assert.Pass();
        }

        [Test]
        public void ResponseShouldNotBeEmptyBecauseOfPlagiarims()
        {
            var response = _plagiarismService.AnalyseCode(_suspectedFunction, _functionsToCompare);
            Assert.AreEqual(response.SimilarFunctions.Count > 0, true);
        }

        [Test]
        public void ResponseShouldBeEmptyBecauseOfNoPlagiarism()
        {
            var noPlagiarizedFunction = GetFunctionAggregate("this is a very different code");
            var response = _plagiarismService.AnalyseCode(noPlagiarizedFunction, _functionsToCompare);
            Assert.AreEqual(response.SimilarFunctions.Count == 0, true);
        }
    }
}