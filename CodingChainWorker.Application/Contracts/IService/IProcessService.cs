using System.Diagnostics;
using Domain.TestExecution;

namespace Application.Contracts.IService
{
    public interface IProcessService<T> where T:ParticipationTestingAggregate
    {
        public void ExecuteParticipationCode(T participation);
        public string WriteParticipation(T participation);
    }
}