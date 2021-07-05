using System.Collections.Generic;

namespace Domain.Plagiarism
{
    public class ComparisonConfig
    {
        public int SamplingWindow { get; set; }
        public int KGramLength { get; set; }
    }

    public interface IPlagiarismSettings
    {
        IList<ComparisonConfig> Configurations { get; set; }
        public double Threshold { get; set; }
    }
}