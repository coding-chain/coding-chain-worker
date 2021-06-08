using System;
using System.Collections.Generic;
using Domain.Plagiarism;
using Domain.Plagiarism.Models;
using Domain.TestExecution;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{
    public class PlagiarismSettingsFake : IPlagiarismSettings
    {
        public IList<ComparisonConfig> Configurations { get; set; }
        public double Threshold { get; set; }
    }

    public class PlagiarismProcessingServiceTests
    {
        private IPlagiarismSettings _fakeSettings;
        private List<FunctionAggregate> _functionsToCompare;
        private PlagiarismService _plagiarismService;
        private FunctionAggregate _suspectedFunctionAggregate;


        [SetUp]
        public void Setup()
        {
            _plagiarismService = new PlagiarismService();
            _functionsToCompare = new List<FunctionAggregate>(new[]
            {
                new FunctionAggregate(new FunctionId(Guid.NewGuid()),
                    "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();var factory = serviceProvider.GetService<ILoggerFactory>();")
            });
            _suspectedFunctionAggregate = new FunctionAggregate(new FunctionId(Guid.NewGuid()),
                "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();");
            _fakeSettings = new PlagiarismSettingsFake
            {
                Threshold = 0.6,
                Configurations = new List<ComparisonConfig>
                {
                    new() {SamplingWindow = 6, KGramLength = 3},
                    new() {SamplingWindow = 8, KGramLength = 5}
                }
            };
        }

        [Test]
        public void ShouldInitTestRight()
        {
            var response =
                _plagiarismService.AnalyseCode(_suspectedFunctionAggregate, _functionsToCompare, _fakeSettings);
            Assert.Pass();
        }

        [Test]
        public void ResponseShouldNotBeEmptyBecauseOfPlagiarism()
        {
            var response =
                _plagiarismService.AnalyseCode(_suspectedFunctionAggregate, _functionsToCompare, _fakeSettings);
            Assert.AreEqual(response.SimilarFunctions.Count > 0, true);
        }

        [Test]
        public void ResponseShouldBeEmptyBecauseOfNoPlagiarism()
        {
            var noPlagiarizedFunction = _suspectedFunctionAggregate;
            noPlagiarizedFunction.Code = "this is a very different code";
            var response = _plagiarismService.AnalyseCode(noPlagiarizedFunction, _functionsToCompare, _fakeSettings);
            Assert.AreEqual(response.SimilarFunctions.Count == 0, true);
        }
    }
}