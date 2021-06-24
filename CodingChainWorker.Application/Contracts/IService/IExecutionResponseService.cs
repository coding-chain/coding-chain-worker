using Application.Read.Code.Execution;

namespace Application.Contracts.IService
{
    public interface IExecutionResponseService
    {
        public void Dispatch(CodeProcessResponse processResponse);
    }
}