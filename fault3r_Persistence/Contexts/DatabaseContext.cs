
using fault3r_Application.Interfaces;
using fault3r_Common;
using fault3r_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace fault3r_Persistence.Contexts
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<Topic> Topics { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>().HasKey(p => p.Id);
            builder.Entity<Account>().HasOne(e => e.Role)
                                     .WithMany(e => e.Accounts)
                                     .HasForeignKey(p => p.RoleId);
            builder.Entity<Account>().HasOne(e => e.Rank)
                                     .WithMany(e => e.Accounts)
                                     .HasForeignKey(p => p.RankId);
            builder.Entity<Account>().HasIndex(p => p.Email).IsUnique();

            builder.Entity<Role>().HasKey(p => p.Id);
            builder.Entity<Role>().HasData(new Role { Id = Guid.Parse(AppRoles.ADMIN), Name = nameof(AppRoles.ADMIN) });
            builder.Entity<Role>().HasData(new Role { Id = Guid.Parse(AppRoles.ACCOUNT), Name = nameof(AppRoles.ACCOUNT) });

            builder.Entity<Rank>().HasKey(p => p.Id);
            builder.Entity<Rank>().HasIndex(p => p.RankNumber).IsUnique();
            builder.Entity<Rank>().HasOne(e => e.Forum)
                                  .WithMany(e => e.Ranks)
                                  .HasForeignKey(p => p.ForumId);
            builder.Entity<Rank>().HasData(new Rank { RankName = AppRoles.DefaultRank, RankNumber = 1 });

            builder.Entity<Forum>().HasKey(p => p.Id);
            builder.Entity<Forum>().HasOne(e => e.ParentForum)
                                   .WithMany(e => e.SubForums)
                                   .HasForeignKey(p => p.ParentForumId);                                

            builder.Entity<Topic>().HasKey(p => p.Id);
            builder.Entity<Topic>().HasOne(e => e.Account)
                                   .WithMany(e => e.Topics)
                                   .HasForeignKey(p => p.AccountId);
            builder.Entity<Topic>().HasOne(e => e.Forum)
                                   .WithMany(e => e.Topics)
                                   .HasForeignKey(p => p.ForumId);                                   
        }
    }
}
