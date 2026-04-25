using Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository;

namespace Clinic_System.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository PatientsRepository { get; }
        IDoctorRepository DoctorsRepository { get; }
        Task<int> SaveAsync();
    }
}
