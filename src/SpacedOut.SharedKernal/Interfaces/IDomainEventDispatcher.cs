using SpacedOut.SharedKernel;
using System.Threading.Tasks;

namespace SpacedOut.SharedKernal.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}
