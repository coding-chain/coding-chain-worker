using System.Threading.Tasks;
using Domain.TestExecution;

namespace Application.Contracts.IService
{
    public interface IProcessService<T> where T:ParticipationTestingAggregate
    {
        public void WriteParticipation(T participation);
        public Task WriteAndExecuteParticipation(T participation);
    }
}