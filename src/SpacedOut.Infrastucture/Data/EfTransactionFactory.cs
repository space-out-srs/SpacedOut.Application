using SpacedOut.SharedKernal.Interfaces;

namespace SpacedOut.Infrastucture.Data
{
    public class EfTransactionFactory : IUnitOfWorkFactory
    {
        private readonly AppDbContext _context;

        public EfTransactionFactory(AppDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork Begin()
        {
            return EfRepository.CreateWithTransaction(_context);
        }
    }
}