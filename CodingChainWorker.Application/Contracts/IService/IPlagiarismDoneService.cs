using Application.PlagiarismAnalyze;

namespace Application.Contracts.IService
{
    public interface IPlagiarismDoneService
    {
        public void Dispatch(PlagiarismAnalyzeResponse plagiarismResponse);
    }
}