using Auxx.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Define your DbSets (tables)
    public DbSet<Roles> Roles { get; set; }

    public DbSet<Auxx.Models.Functions> Functions { get; set; } = default!;

    public DbSet<Auxx.Models.Organizations> Organizations { get; set; } = default!;

    public DbSet<Auxx.Models.OrganizationReports> OrganizationReports { get; set; } = default!;

    public DbSet<Auxx.Models.Invoices> Invoices { get; set; } = default!;

    public DbSet<Auxx.Models.FunctionAccessControl> FunctionAccessControl { get; set; } = default!;

    public DbSet<Auxx.Models.OrganizationLevelAccess> OrganizationLevelAccess { get; set; } = default!;

    public DbSet<Auxx.Models.Reportdetails> Reportdetails { get; set; } = default!;

    public DbSet<Auxx.Models.User> User { get; set; } = default!;

    public DbSet<Auxx.Models.Levels> Levels { get; set; } = default!;

    public DbSet<Auxx.Models.CGStagingData> cgstagingdata { get; set; } = default!;
}