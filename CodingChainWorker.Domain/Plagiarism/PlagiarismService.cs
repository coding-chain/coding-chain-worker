using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Domain.Contracts;
using Domain.Plagiarism.Models;

namespace Domain.Plagiarism
{
    public class PlagiarismService : ICodePlagiarismService
    {
        private IPlagiarismSettings? _settings;
        private readonly IHashService _hashService;

        public PlagiarismService(IPlagiarismSettings? settings, IHashService hashService)
        {
            _settings = settings;
            _hashService = hashService;
        }

        public FunctionAggregate AnalyseCode(FunctionAggregate suspectedFunctionAggregate,
            IList<FunctionAggregate> functionsToCompare)
        {
            foreach (var functionToCompare in functionsToCompare)
            {
                if (functionToCompare.Id.Equals(suspectedFunctionAggregate.Id)) continue;

                var averageSimilarity =
                    GetAverageSimilarityRate(suspectedFunctionAggregate.Code, functionToCompare.Code);
                if (averageSimilarity > _settings.Threshold)
                    suspectedFunctionAggregate.AddSimilarFunction(
                        functionToCompare.Id,
                        functionToCompare.Hash,
                        averageSimilarity);
            }

            return suspectedFunctionAggregate;
        }

        private double GetAverageSimilarityRate(string suspectedFunctionCode, string comparedFunctionCode)
        {
            return _settings.Configurations.Average(config => GetSimilarityRate(
                config.SamplingWindow,
                config.KGramLength,
                suspectedFunctionCode, comparedFunctionCode));
        }

        private double GetSimilarityRate(int t, int k, string suspectedFunctionCode, string comparedFunctionCode)
        {
            var suspectedList = SampleFingerprints(ExtractFingerprints(suspectedFunctionCode, k), t + 1 - k);
            var comparedList = SampleFingerprints(ExtractFingerprints(comparedFunctionCode, k), t + 1 - k);
            var rate = (double)GetIntersection(suspectedList, comparedList).Count / comparedList.Count;
            return rate;
        }

        private List<string> SampleFingerprints(List<string> rawFingerPrints, int w)
        {
            var sampledFingerprints = new List<string>();
            var minIndex = -1;
            var preMinIndex = -1;

            for (var i = 0; i < rawFingerPrints.Count - w; i++)
            {
                string? tmpMin = null;
                for (var j = i; j < i + w; j++)
                    if (tmpMin == null || rawFingerPrints[j].Length <= tmpMin.Length)
                    {
                        minIndex = j;
                        tmpMin = rawFingerPrints[j];
                    }

                if (minIndex != preMinIndex)
                {
                    preMinIndex = minIndex;
                    sampledFingerprints.Add(rawFingerPrints[minIndex]);
                }
            }

            return sampledFingerprints;
        }

        private List<string> ExtractFingerprints(string content, int k)
        {
            var fingerprints = new List<string>();
            for (var i = 0; i < content.Length - k; i++)
            {
                string kgram = content.Substring(i, k);
                string hash = _hashService.GetHash( kgram);
                fingerprints.Add(hash);
            }

            return fingerprints;
        }

        private List<string> GetIntersection(List<string> suspectedList, List<string> comparedList)
        {
            var intersections = new List<string>();
            foreach (var element in comparedList)
                if (suspectedList.Contains(element))
                    intersections.Add(element);

            return intersections;
        }

        public FunctionAggregate AnalyseCode(FunctionAggregate suspectedFunctionAggregate, IList<FunctionAggregate> functionsToCompare,
            IPlagiarismSettings settings)
        {
            throw new System.NotImplementedException();
        }
    }
}