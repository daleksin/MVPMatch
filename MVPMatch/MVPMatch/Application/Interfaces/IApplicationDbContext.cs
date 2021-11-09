using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MVPMatch.Core.Models;

namespace MVPMatch.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
