using Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository;

namespace Clinic_System.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDoctorRepository DoctorsRepository { get; }
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
