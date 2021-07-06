using System.Threading.Tasks;

namespace Application.Contracts.IService
{
    public interface IDispatcher<TMessage>
    {
        public Task Dispatch(TMessage message);
    }
}