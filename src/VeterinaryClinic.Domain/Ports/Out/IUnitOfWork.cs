
namespace VeterinaryClinic.Domain.Ports.Out
{
    public interface IUnitOfWork : IDisposable
    {
        IOwnerRepository Owners { get; }
        IPetRepository Pets { get; }
        IAppointmentRepository Appointments { get; }
        IUserRepository Users { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}