
namespace VeterinaryClinic.Domain.Ports.Out
{
    public interface IUnitOfWork : IDisposable
    {
        IOwnerRepository Owners { get; }
        IPetRepository Pets { get; }
        IAppointmentRepository Appointments { get; }

        Task<int> SaveCahngesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}