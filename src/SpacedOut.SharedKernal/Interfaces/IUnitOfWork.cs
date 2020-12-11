using System;
using System.Threading.Tasks;

namespace SpacedOut.SharedKernal.Interfaces
{
    public interface IUnitOfWork : IDisposable, IRepository
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
