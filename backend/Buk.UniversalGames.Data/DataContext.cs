using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Matches;
using Buk.UniversalGames.Data.Models.SideQuest;
using Buk.UniversalGames.Library.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using System.Xml.Linq;

namespace Buk.UniversalGames.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<League> Leagues{ get; init; }
    public DbSet<Team> Teams { get; init; }
    public DbSet<Sticker> Stickers { get; init; }
    public DbSet<StickerScan> StickerScans { get; init; }
    public DbSet<PointsRegistration> Points{ get; init; }
    public DbSet<Game> Games { get; init; }
    public DbSet<Match> Matches { get; init; }
    public DbSet<Guess> Guesses { get; init; }
    public DbSet<Settings> Settings { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<GameType>();
        builder.HasPostgresEnum<TeamType>();

        builder.Entity<League>().HasKey(t => t.LeagueId);

        builder.Entity<Team>().HasKey(t => t.TeamId);
        builder.Entity<Team>().HasIndex(t => t.Code);

        builder.Entity<Sticker>().HasKey(t => t.StickerId);
        builder.Entity<Sticker>().HasIndex(t => t.Code);

        builder.Entity<StickerScan>().HasKey(t => t.StickerScanId);
        builder.Entity<StickerScan>().Property(e => e.At).HasColumnType("timestamp without time zone");

        builder.Entity<PointsRegistration>().HasKey(t => t.PointId);

        builder.Entity<PointsRegistration>().Property(e => e.Added).HasColumnType("timestamp without time zone");

        builder.Entity<Game>().HasKey(t => t.GameId);

        builder.Entity<Match>().HasKey(t => t.MatchId);
        builder.Entity<Match>().Property(e => e.Start).HasColumnType("timestamp without time zone");

        builder.Entity<Guess>().HasKey(g => g.Id);
        builder.Entity<Guess>().Property(g => g.Time).HasColumnType("timestamp without time zone");

        builder.Entity<Settings>().HasKey(t => t.Key);

        base.OnModelCreating(builder);
    }

    public class ContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql($"Host=localhost;Database=buk-universal-games;Username=admin;Password=password;Timeout=300;CommandTimeout=300");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
