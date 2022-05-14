using Microsoft.EntityFrameworkCore;

namespace Buk.UniversalGames.Api;


public class DataContext : DbContext
{
    public DbSet<Team> Teams { get; set; }

    public static string GetConnectionString() {
        var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var connectionString = $"Host={dbHost}:{dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Timeout=300;CommandTimeout=300";
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {     

        options.UseNpgsql(GetConnectionString())
                .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Team>().HasKey(t => t.TeamId);

        base.OnModelCreating(builder);
    }
}
