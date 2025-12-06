using Microsoft.EntityFrameworkCore.Storage;
using VeterinaryClinic.Domain.Ports.Out;
using VeterinaryClinic.Infrastructure.Persistence.Context;

namespace VeterinaryClinic.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IOwnerRepository Owners { get; }
        public IPetRepository Pets { get; }
        public IAppointmentRepository Appointments { get; }

        public UnitOfWork(ApplicationDbContext context,
                          IOwnerRepository ownerRepository,
                          IPetRepository petRepository,
                          IAppointmentRepository appointmentRepository)
        {
            _context = context;
            Owners = ownerRepository;
            Pets = petRepository;
            Appointments = appointmentRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}