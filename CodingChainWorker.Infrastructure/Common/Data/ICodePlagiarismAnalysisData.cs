using System;

namespace CodingChainApi.Infrastructure.Common.Data
{
    public interface ICodePlagiarismAnalysisData
    {
        public const double THRESHOLD = 0.6;

        public enum NARROW_CONFIG
        {
            SAMPLING_THRESHOLD = 6,
            K_GRAM_LENGTH = 3
        }

        public enum BROAD_CONFIG
        {
            SAMPLING_THRESHOLD = 8,
            K_GRAM_LENGTH = 5
        }
    }
}