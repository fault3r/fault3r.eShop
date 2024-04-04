
using fault3r_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace fault3r_Application.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<Rank> Ranks { get; set; }

        DbSet<Forum> Forums { get; set; }

        DbSet<Topic> Topics { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
