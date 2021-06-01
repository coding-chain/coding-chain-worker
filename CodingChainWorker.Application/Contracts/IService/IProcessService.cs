using System.Threading.Tasks;
using Application.Read.Execution;
using Domain.TestExecution;

namespace Application.Contracts.IService
{
    public interface IProcessService<T> where T:ParticipationTestingAggregate
    {
        public Task<CodeProcessResponse> ExecuteParticipation(T participation);
        public void WriteParticipation(T participation);
        public Task<CodeProcessResponse> WriteAndExecuteParticipation(T participation);
    }
}