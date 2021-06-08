namespace CodingChainApi.Infrastructure.Common.Data
{
    public interface ICodePlagiarismAnalysisData
    {
        public enum BROAD_CONFIG
        {
            SAMPLING_THRESHOLD = 8,
            K_GRAM_LENGTH = 5
        }

        public enum NARROW_CONFIG
        {
            SAMPLING_THRESHOLD = 6,
            K_GRAM_LENGTH = 3
        }

        public const double THRESHOLD = 0.6;
    }
}