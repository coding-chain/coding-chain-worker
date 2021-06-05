using System.Collections.Generic;
using Domain.Plagiarism;

namespace CodingChainApi.Infrastructure.Settings
{

    public class PlagiarismSettings : IPlagiarismSettings
    {
        public IList<ComparisonConfig> Configurations { get; set; } = new List<ComparisonConfig>();
        public double Threshold { get; set; }
    }
}