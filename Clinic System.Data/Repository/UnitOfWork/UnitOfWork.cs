
using Clinic_System.Core.Exceptions;
using Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository;
using Clinic_System.Core.Interfaces.UnitOfWork;
using Clinic_System.Data.Context;
using Clinic_System.Data.Repository.RepositoriesForEntities;
using Microsoft.Data.SqlClient;

namespace Clinic_System.Data.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AppDbContext context;
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }
        IDoctorRepository DoctorsRepo;

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


        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627 || sqlEx.Number == 1505)
                    {
                        throw new UniqueConstraintViolationException("A database unique constraint was violated.", ex);
                    }
                }

                throw;
            }
        }
    }
}
