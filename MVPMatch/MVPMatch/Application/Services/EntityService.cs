using MVPMatch.Application.Interfaces;
using MVPMatch.Common.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVPMatch.Application.Services
{
    public interface IEntityService
    {
        Task Delete<TEntity>(int id) where TEntity : class;
    }

    public class EntityService : IEntityService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public EntityService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Delete<TEntity>(int id) where TEntity : class
        {
            var entity =
                await _applicationDbContext.Set<TEntity>().FindAsync(id)
                ?? throw new NotFoundException(nameof(TEntity), id);

            try
            {
                _applicationDbContext.Set<TEntity>().Remove(entity);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DeleteFailureException(nameof(TEntity), id, ex.Message);
            }
        }
    }
}
