using CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Data.Configurtion.Contracts
{
    public interface IApplicationDbContext
    {
        DbSet<Todo> Todos { get; set; }
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
