using CQRS.Data.Configurtion;
using CQRS.Data.Configurtion.Contracts;
using CQRS.Domain.Common;
using CQRS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Data.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<Users, Roles, int>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public DbSet<Todo> Todos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDateTime dateTime)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var assembly = typeof(ApplicationDbContext).Assembly;
            builder.RegisterEntityTypeConfiguration(assembly);
            base.OnModelCreating(builder);
            //builder.RegisterAllEntities<IBaseEntity>(assembly);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.UserCreatorId = _currentUserService.UserId;
                        entry.Entity.CreatedAt = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UserEditorId = _currentUserService.UserId;
                        entry.Entity.UpdatedAt = _dateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return SaveChangesAsync(cancellationToken);
        }
    }
}
