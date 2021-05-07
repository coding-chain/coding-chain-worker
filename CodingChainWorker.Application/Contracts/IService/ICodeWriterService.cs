using Domain.TestExecution;

namespace Application.Contracts.IService
{
    public interface ICodeWriterService
    {
        string WriteParticipation(ParticipationTestingAggregate participation);
    }
}