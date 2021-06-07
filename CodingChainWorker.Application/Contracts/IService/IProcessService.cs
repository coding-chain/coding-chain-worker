using System.Threading.Tasks;
using Domain.TestExecution;

namespace Application.Contracts.IService
{
    public interface IProcessService
    {
        public void WriteParticipation(ParticipationAggregate participation);
        public Task WriteAndExecuteParticipation(ParticipationAggregate participation);
        public Task PrepareParticipationExecution(ParticipationAggregate participation);
        public Task CleanParticipationExecution(ParticipationAggregate participation);
    }
}