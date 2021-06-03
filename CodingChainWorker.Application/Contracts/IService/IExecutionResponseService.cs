using Application.Contracts.Processes;

namespace Application.Contracts.IService
{
    public interface IExecutionResponseService
    {
        public void Dispatch(CodeProcessResponse processResponse);
    }
}