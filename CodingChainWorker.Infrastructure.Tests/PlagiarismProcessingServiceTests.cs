using System;
using System.Collections.Generic;
using CodingChainApi.Infrastructure.Models;
using CodingChainApi.Infrastructure.Services.CodeAnalysis.Plagiarism;
using NUnit.Framework;

namespace CodingChainWorker.Infrastructure.Tests
{
    public class PlagiarismProcessingServiceTests
    {
        private PlagiarismService _plagiarismService;
        private List<Function> _functionsToCompare;
        private Function _suspectedFunction;

        [SetUp]
        public void Setup()
        {
            _plagiarismService = new PlagiarismService();
            _functionsToCompare = new List<Function>(new[]
            {
                new Function(Guid.NewGuid(),
                    "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();var factory = serviceProvider.GetService<ILoggerFactory>();",
                    1, false)
            });
            _suspectedFunction = new Function(Guid.NewGuid(),
                "var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();", 2, false);
        }

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
            Assert.AreEqual(response.IsTherePlagiarism(), true);
        }

        [Test]
        public void ResponseShouldBeEmptyBecauseOfNoPlagiarism()
        {
            var noPlagiarizedFunction = _suspectedFunction;
            noPlagiarizedFunction.Code = "this is a very different code";
            var response = _plagiarismService.AnalyseCode(noPlagiarizedFunction, _functionsToCompare);
            Assert.AreEqual(response.IsTherePlagiarism(), false);
        }
    }
}