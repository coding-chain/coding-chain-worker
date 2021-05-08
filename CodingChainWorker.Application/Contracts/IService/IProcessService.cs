using Application.Contracts.Processes;
using Domain.TestExecution;

namespace Application.Contracts.IService
{
    public interface IProcessService<T> where T:ParticipationTestingAggregate
    {
        public IProcessEndHandler ExecuteParticipation(T participation);
        public void WriteParticipation(T participation);
        public IProcessEndHandler WriteAndExecuteParticipation(T participation);
    }
}