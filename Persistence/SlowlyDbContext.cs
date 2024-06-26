using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Persistence;

public class SlowlyDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IUnitOfWork
{
    public DbSet<SlowlyUser> SlowlyUsers { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<QueuedEmail> QueuedEmails { get; set; }
    public DbSet<UserTopic> UserTopics { get; set; }
    public DbSet<UserLanguage> UserLanguages { get; set; }
    public DbSet<Letter> Letters { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Stamp> Stamps { get; set; }
    public DbSet<MatchingPreferences> MatchingPreferences { get; set; }

    private IDbContextTransaction _dbContextTransaction;
    //public SlowlyDbContext(DbContextOptions options) : base(options) { }

    public SlowlyDbContext(DbContextOptions<SlowlyDbContext> options) : base(options)
    {
    }

    public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _dbContextTransaction;
    }

    public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, string lockName = null, CancellationToken cancellationToken = default)
    {
        _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return _dbContextTransaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContextTransaction.CommitAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

