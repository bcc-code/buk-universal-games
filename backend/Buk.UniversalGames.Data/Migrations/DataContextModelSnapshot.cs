﻿// <auto-generated />
using System;
using Buk.UniversalGames.Data;
using Buk.UniversalGames.Library.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Buk.UniversalGames.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "game_type", new[] { "unknown", "water", "wood", "fire", "earth", "metal" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "team_type", new[] { "unknown", "participant", "admin", "system_admin" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameId"));

                    b.Property<int>("LooserPoints")
                        .HasColumnType("integer")
                        .HasColumnName("looser_points");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<GameType>("Type")
                        .HasColumnType("game_type")
                        .HasColumnName("type");

                    b.Property<int>("WinnerPoints")
                        .HasColumnType("integer")
                        .HasColumnName("winner_points");

                    b.HasKey("GameId")
                        .HasName("pk_games");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.League", b =>
                {
                    b.Property<int>("LeagueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("league_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LeagueId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("LeagueId")
                        .HasName("pk_leagues");

                    b.ToTable("leagues", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("match_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MatchId"));

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start");

                    b.Property<int>("Team1Id")
                        .HasColumnType("integer")
                        .HasColumnName("team1id");

                    b.Property<int>("Team1TeamTeamId")
                        .HasColumnType("integer")
                        .HasColumnName("team1team_team_id");

                    b.Property<int>("Team2Id")
                        .HasColumnType("integer")
                        .HasColumnName("team2id");

                    b.Property<int>("Team2TeamTeamId")
                        .HasColumnType("integer")
                        .HasColumnName("team2team_team_id");

                    b.Property<int>("WinnerId")
                        .HasColumnType("integer")
                        .HasColumnName("winner_id");

                    b.Property<int>("WinnerTeamTeamId")
                        .HasColumnType("integer")
                        .HasColumnName("winner_team_team_id");

                    b.HasKey("MatchId")
                        .HasName("pk_matches");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_matches_game_id");

                    b.HasIndex("Team1TeamTeamId")
                        .HasDatabaseName("ix_matches_team1team_team_id");

                    b.HasIndex("Team2TeamTeamId")
                        .HasDatabaseName("ix_matches_team2team_team_id");

                    b.HasIndex("WinnerTeamTeamId")
                        .HasDatabaseName("ix_matches_winner_team_team_id");

                    b.ToTable("matches", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Point", b =>
                {
                    b.Property<int>("PointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("point_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PointId"));

                    b.Property<DateTime>("Added")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("added")
                        .HasDefaultValueSql("now()");

                    b.Property<int?>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<int?>("MatchId")
                        .HasColumnType("integer")
                        .HasColumnName("match_id");

                    b.Property<int>("Points")
                        .HasColumnType("integer")
                        .HasColumnName("points");

                    b.Property<int?>("StickerId")
                        .HasColumnType("integer")
                        .HasColumnName("sticker_id");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer")
                        .HasColumnName("team_id");

                    b.HasKey("PointId")
                        .HasName("pk_points");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_points_game_id");

                    b.HasIndex("MatchId")
                        .HasDatabaseName("ix_points_match_id");

                    b.HasIndex("StickerId")
                        .HasDatabaseName("ix_points_sticker_id");

                    b.HasIndex("TeamId")
                        .HasDatabaseName("ix_points_team_id");

                    b.ToTable("points", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Sticker", b =>
                {
                    b.Property<int>("StickerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sticker_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StickerId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<int>("LeagueId")
                        .HasColumnType("integer")
                        .HasColumnName("league_id");

                    b.Property<int>("Points")
                        .HasColumnType("integer")
                        .HasColumnName("points");

                    b.HasKey("StickerId")
                        .HasName("pk_stickers");

                    b.HasIndex("LeagueId")
                        .HasDatabaseName("ix_stickers_league_id");

                    b.ToTable("stickers", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.StickerScan", b =>
                {
                    b.Property<int>("StickerScanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sticker_scan_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StickerScanId"));

                    b.Property<DateTime>("At")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("at")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("StickerId")
                        .HasColumnType("integer")
                        .HasColumnName("sticker_id");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer")
                        .HasColumnName("team_id");

                    b.HasKey("StickerScanId")
                        .HasName("pk_sticker_scans");

                    b.HasIndex("StickerId")
                        .HasDatabaseName("ix_sticker_scans_sticker_id");

                    b.HasIndex("TeamId")
                        .HasDatabaseName("ix_sticker_scans_team_id");

                    b.ToTable("sticker_scans", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("team_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<int?>("LeagueId")
                        .HasColumnType("integer")
                        .HasColumnName("league_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<TeamType>("Type")
                        .HasColumnType("team_type")
                        .HasColumnName("type");

                    b.HasKey("TeamId")
                        .HasName("pk_teams");

                    b.HasIndex("LeagueId")
                        .HasDatabaseName("ix_teams_league_id");

                    b.ToTable("teams", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Match", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_games_game_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team1Team")
                        .WithMany()
                        .HasForeignKey("Team1TeamTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_teams_team1team_team_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team2Team")
                        .WithMany()
                        .HasForeignKey("Team2TeamTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_teams_team2team_team_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "WinnerTeam")
                        .WithMany()
                        .HasForeignKey("WinnerTeamTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_teams_winner_team_team_id");

                    b.Navigation("Game");

                    b.Navigation("Team1Team");

                    b.Navigation("Team2Team");

                    b.Navigation("WinnerTeam");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Point", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("fk_points_games_game_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Match", "Match")
                        .WithMany()
                        .HasForeignKey("MatchId")
                        .HasConstraintName("fk_points_matches_match_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Sticker", "Sticker")
                        .WithMany()
                        .HasForeignKey("StickerId")
                        .HasConstraintName("fk_points_stickers_sticker_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team")
                        .WithMany("Points")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_points_teams_team_id");

                    b.Navigation("Game");

                    b.Navigation("Match");

                    b.Navigation("Sticker");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Sticker", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.League", "League")
                        .WithMany("Stickers")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stickers_leagues_league_id");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.StickerScan", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.Sticker", "Sticker")
                        .WithMany()
                        .HasForeignKey("StickerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sticker_scans_stickers_sticker_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team")
                        .WithMany("StickerScans")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sticker_scans_teams_team_id");

                    b.Navigation("Sticker");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Team", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .HasConstraintName("fk_teams_leagues_league_id");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.League", b =>
                {
                    b.Navigation("Stickers");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Team", b =>
                {
                    b.Navigation("Points");

                    b.Navigation("StickerScans");
                });
#pragma warning restore 612, 618
        }
    }
}
