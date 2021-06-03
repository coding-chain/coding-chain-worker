using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodingChainApi.Infrastructure.Common.Data;
using CodingChainApi.Infrastructure.Models;
using CodingChainApi.Infrastructure.Settings;
using CodingChainApi.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;

namespace CodingChainApi.Infrastructure.Services.CodeAnalysis.Plagiarism
{
    public class PlagiarismService : ICodePlagiarismService
    {
        public CodePlagiarismReponse AnalyseCode(Function suspectedFunction, IList<Function> functionsToCompare)
        {
            CodePlagiarismReponse response = new CodePlagiarismReponse();
            suspectedFunction.Code = Regex.Replace(suspectedFunction.Code, @"\s+", " ");
            foreach (var functionToCompare in functionsToCompare)
            {
                if (functionToCompare.Id.Equals(suspectedFunction.Id))
                {
                    continue;
                }

                functionToCompare.Code = Regex.Replace(functionToCompare.Code, @"\s+", " ");
                var averageSimilarity = GetAverageSimilarityRate(suspectedFunction.Code, functionToCompare.Code);
                if (averageSimilarity > ICodePlagiarismAnalysisData.THRESHOLD)
                {
                    response.addPlagiarizedFunction(suspectedFunction.Id, functionToCompare.Id,
                        String.Format("{0:0.00}", averageSimilarity));
                }
            }

            return response;
        }

        private double GetAverageSimilarityRate(string suspectedFunctionCode, string comparedFunctionCode)
        {
            var narrowSimilarity = GetSimilarityRate((int) ICodePlagiarismAnalysisData.NARROW_CONFIG.SAMPLING_THRESHOLD,
                (int) ICodePlagiarismAnalysisData.NARROW_CONFIG.K_GRAM_LENGTH, suspectedFunctionCode,
                comparedFunctionCode);
            var broadSimilarity = GetSimilarityRate((int) ICodePlagiarismAnalysisData.BROAD_CONFIG.SAMPLING_THRESHOLD,
                (int) ICodePlagiarismAnalysisData.BROAD_CONFIG.K_GRAM_LENGTH, suspectedFunctionCode,
                comparedFunctionCode);
            return (narrowSimilarity + broadSimilarity) / 2;
        }

        private double GetSimilarityRate(int t, int k, string suspectedFunctioncode, string comparedFunctionCode)
        {
            var suspectedList = SampleFingerprints(ExtractFingerprints(suspectedFunctioncode, k), t + 1 - k);
            var comparedList = SampleFingerprints(ExtractFingerprints(comparedFunctionCode, k), t + 1 - k);
            var rate = (double) GetIntersection(suspectedList, comparedList).Count / comparedList.Count;
            return rate;
        }

        private List<string> SampleFingerprints(List<string> rawFingerPrints, int w)
        {
            List<string> sampledFingerprints = new List<string>();
            int minIndex = -1;
            int preMinIndex = -1;

            for (int i = 0; i < rawFingerPrints.Count - w; i++)
            {
                string? tmpMin = null;
                for (int j = i; j < i + w; j++)
                {
                    if (tmpMin == null || rawFingerPrints[j].Length <= tmpMin.Length)
                    {
                        minIndex = j;
                        tmpMin = rawFingerPrints[j];
                    }
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
            List<string> fingerprints = new List<string>();
            for (int i = 0; i < content.Length - k; i++)
            {
                string kgram = content.Substring(i, k);
                string hash = HashUtils.GetHash(SHA256.Create(), kgram);
                fingerprints.Add(hash);
            }

            return fingerprints;
        }

        private List<string> GetIntersection(List<string> suspectedList, List<string> comparedList)
        {
            List<string> intersections = new List<string>();
            foreach (var element in comparedList)
            {
                if (suspectedList.Contains(element))
                {
                    intersections.Add(element);
                }
            }

            return intersections;
        }
    }
}