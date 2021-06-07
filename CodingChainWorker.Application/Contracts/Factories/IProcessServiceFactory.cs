using Application.Contracts.IService;
using Domain.TestExecution;

namespace Application.Contracts
{
    public interface IProcessServiceFactory
    {
        IProcessService GetProcessServiceByLanguage(LanguageEnum language);
    }
}