using SpacedOut.SharedKernel;
using System.Threading.Tasks;

namespace SpacedOut.SharedKernal.Interfaces
{
    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}