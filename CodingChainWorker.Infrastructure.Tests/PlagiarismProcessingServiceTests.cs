using System;
using System.Collections.Generic;
using CodingChainApi.Infrastructure.Settings;
using Domain.Plagiarism;
using Domain.Plagiarism.Models;
using Domain.TestExecution;
using NUnit.Framework;

namespace CodingChainWorker.Infrastructure.Tests
{
    public class PlagiarismSettingsFake : IPlagiarismSettings
    {
        public IList<ComparisonConfig> Configurations { get; set; }
        public double Threshold { get; set; }
    }

    public class PlagiarismProcessingServiceTests
    {
        private List<FunctionAggregate> _functionsToCompare;
        private PlagiarismService _plagiarismService;
        private PlagiarismSettings _settings;
        private FunctionAggregate _suspectedFunction;

        [SetUp]
        public void Setup()
        {
            _settings = new PlagiarismSettings
            {
                Threshold = 0.6,
                Configurations = new List<ComparisonConfig>
                {
                    new() {SamplingWindow = 6, KGramLength = 3},
                    new() {SamplingWindow = 8, KGramLength = 5}
                }
            };
            _plagiarismService = new PlagiarismService();
            _functionsToCompare = new List<FunctionAggregate>(new[]
            {
                new FunctionAggregate(new FunctionId(Guid.NewGuid()),
                    "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();var factory = serviceProvider.GetService<ILoggerFactory>();"
                )
            });
            _suspectedFunction = new FunctionAggregate(new FunctionId(Guid.NewGuid()),
                "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();");
        }

        [Test]
        public void ShouldInitTestRight()
        {
            var response = _plagiarismService.AnalyseCode(_suspectedFunction, _functionsToCompare, _settings);
            Assert.Pass();
        }

        [Test]
        public void ResponseShouldNotBeEmptyBecauseOfPlagiarims()
        {
            var response = _plagiarismService.AnalyseCode(_suspectedFunction, _functionsToCompare, _settings);
            Assert.AreEqual(response.SimilarFunctions.Count > 0, true);
        }

        [Test]
        public void ResponseShouldBeEmptyBecauseOfNoPlagiarism()
        {
            var noPlagiarizedFunction = _suspectedFunction;
            noPlagiarizedFunction.Code = "this is a very different code";
            var response = _plagiarismService.AnalyseCode(noPlagiarizedFunction, _functionsToCompare, _settings);
            Assert.AreEqual(response.SimilarFunctions.Count == 0, true);
        }
    }
}