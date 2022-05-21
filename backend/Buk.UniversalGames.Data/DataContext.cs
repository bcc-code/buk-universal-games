using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Library.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Buk.UniversalGames.Data;

public class DataContext : DbContext
{
    public DbSet<League> Leagues{ get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Sticker> Stickers { get; set; }
    public DbSet<StickerScan> StickerScans { get; set; }
    public DbSet<Point> Points{ get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Match> Matches { get; set; }

    static DataContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<GameType>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<TeamType>();
    }

    public static string GetConnectionString() {
        var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        return $"Host={dbHost}{(dbPort != "5432" ? ";Port=" + (dbPort ?? "") : "")};Database={dbName};Username={dbUser};Password={dbPassword};Timeout=300;CommandTimeout=300";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {     

        options.UseNpgsql(GetConnectionString())
                .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<GameType>();
        builder.HasPostgresEnum<TeamType>();

        builder.Entity<League>().HasKey(t => t.LeagueId);

        builder.Entity<Team>().HasKey(t => t.TeamId);

        builder.Entity<Sticker>().HasKey(t => t.StickerId);

        builder.Entity<StickerScan>().HasKey(t => t.StickerScanId);
        builder.Entity<StickerScan>().Property(e => e.At).HasDefaultValueSql("now()");

        builder.Entity<Point>().HasKey(t => t.PointId);
        builder.Entity<Point>().Property(e => e.Added).HasDefaultValueSql("now()");

        builder.Entity<Game>().HasKey(t => t.GameId);

        builder.Entity<Match>().HasKey(t => t.MatchId);

        base.OnModelCreating(builder);
    }
}
