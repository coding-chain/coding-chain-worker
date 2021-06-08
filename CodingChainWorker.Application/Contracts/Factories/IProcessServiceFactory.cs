using Application.Contracts.IService;
using Domain.TestExecution;

namespace Application.Contracts.Factories
{
    public interface IProcessServiceFactory
    {
        IProcessService GetProcessServiceByLanguage(LanguageEnum language);
    }
}