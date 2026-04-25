using Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository;
using Clinic_System.Core.Interfaces.UnitOfWork;
using Clinic_System.Data.Context;
using Clinic_System.Data.Repository.RepositoriesForEntities;

namespace Clinic_System.Data.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }
        IPatientRepository PatientsRepo;

        IDoctorRepository DoctorsRepo;


        public IPatientRepository PatientsRepository
        {
            get
            {
                if (PatientsRepo == null)
                {
                    PatientsRepo = new PatientRepository(context);
                }
                return PatientsRepo;
            }
        }

        public IDoctorRepository DoctorsRepository
        {
            get
            {
                if (DoctorsRepo == null)
                {
                    DoctorsRepo = new DoctorRepository(context);
                }
                return DoctorsRepo;
            }
        }


        public void Dispose() => context.Dispose();

        public Task<int> SaveAsync() => context.SaveChangesAsync();


    }
}