using Application.Contracts.Processes;

namespace Application.Contracts.IService
{
    public interface IParticipationDoneService
    {
        public void Dispatch(CodeProcessResponse processResponse);
    }
}