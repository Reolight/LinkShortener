using EFCoreStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreStore;

internal class AppDbContext : DbContext
{
    public DbSet<Url> Urls { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
}