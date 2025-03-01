﻿// <auto-generated />
using System;
using Buk.UniversalGames.Data;
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
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "game_type", new[] { "land_water_beach", "labyrinth", "hamster_wheel", "mastermind", "iron_grip" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "team_type", new[] { "participant", "admin", "system_admin" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Family", b =>
                {
                    b.Property<int>("FamilyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("family_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FamilyId"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("color");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("FamilyId")
                        .HasName("pk_families");

                    b.ToTable("families", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("game_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("GameType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<int>("LooserPoints")
                        .HasColumnType("integer")
                        .HasColumnName("looser_points");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ParticipantsInfo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("participants_info");

                    b.Property<string>("SafetyInfo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("safety_info");

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
                        .HasColumnName("league_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LeagueId"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("color");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("LeagueId")
                        .HasName("pk_leagues");

                    b.ToTable("leagues", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Matches.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("match_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MatchId"));

                    b.Property<string>("AddOn")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("add_on");

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<int>("LeagueId")
                        .HasColumnType("integer")
                        .HasColumnName("league_id");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("position");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("start");

                    b.Property<int>("Team1Id")
                        .HasColumnType("integer")
                        .HasColumnName("team1id");

                    b.Property<int>("Team2Id")
                        .HasColumnType("integer")
                        .HasColumnName("team2id");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("integer")
                        .HasColumnName("winner_id");

                    b.HasKey("MatchId")
                        .HasName("pk_matches");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_matches_game_id");

                    b.HasIndex("LeagueId")
                        .HasDatabaseName("ix_matches_league_id");

                    b.HasIndex("Team1Id")
                        .HasDatabaseName("ix_matches_team1id");

                    b.HasIndex("Team2Id")
                        .HasDatabaseName("ix_matches_team2id");

                    b.HasIndex("WinnerId")
                        .HasDatabaseName("ix_matches_winner_id");

                    b.ToTable("matches", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.PointsRegistration", b =>
                {
                    b.Property<int>("PointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("point_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PointId"));

                    b.Property<DateTime>("Added")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("added");

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

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Settings", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Key")
                        .HasName("pk_settings");

                    b.ToTable("settings", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.SideQuest.Guess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("answer");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean")
                        .HasColumnName("is_correct");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer")
                        .HasColumnName("question_id");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer")
                        .HasColumnName("team_id");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time");

                    b.Property<DateTime>("TimeAnswered")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time_answered");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("unique_id");

                    b.HasKey("Id")
                        .HasName("pk_guesses");

                    b.HasIndex("TeamId")
                        .HasDatabaseName("ix_guesses_team_id");

                    b.ToTable("guesses", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Sticker", b =>
                {
                    b.Property<int>("StickerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sticker_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

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

                    b.HasIndex("Code")
                        .HasDatabaseName("ix_stickers_code");

                    b.HasIndex("LeagueId")
                        .HasDatabaseName("ix_stickers_league_id");

                    b.ToTable("stickers", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.StickerScan", b =>
                {
                    b.Property<int>("StickerScanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sticker_scan_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StickerScanId"));

                    b.Property<DateTime>("At")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("at");

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
                        .HasColumnName("team_id")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Color")
                        .HasColumnType("text")
                        .HasColumnName("color");

                    b.Property<int?>("FamilyId")
                        .HasColumnType("integer")
                        .HasColumnName("family_id");

                    b.Property<int?>("LeagueId")
                        .HasColumnType("integer")
                        .HasColumnName("league_id");

                    b.Property<int>("MemberCount")
                        .HasColumnType("integer")
                        .HasColumnName("member_count");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("TeamType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("TeamId")
                        .HasName("pk_teams");

                    b.HasIndex("Code")
                        .HasDatabaseName("ix_teams_code");

                    b.HasIndex("FamilyId")
                        .HasDatabaseName("ix_teams_family_id");

                    b.HasIndex("LeagueId")
                        .HasDatabaseName("ix_teams_league_id");

                    b.ToTable("teams", (string)null);
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Matches.Match", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_games_game_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_leagues_league_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team1")
                        .WithMany()
                        .HasForeignKey("Team1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_teams_team1id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team2")
                        .WithMany()
                        .HasForeignKey("Team2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_matches_teams_team2id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .HasConstraintName("fk_matches_teams_winner_id");

                    b.Navigation("Game");

                    b.Navigation("League");

                    b.Navigation("Team1");

                    b.Navigation("Team2");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.PointsRegistration", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("fk_points_games_game_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.Matches.Match", "Match")
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

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.SideQuest.Guess", b =>
                {
                    b.HasOne("Buk.UniversalGames.Data.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_guesses_teams_team_id");

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
                    b.HasOne("Buk.UniversalGames.Data.Models.Family", "Family")
                        .WithMany("Teams")
                        .HasForeignKey("FamilyId")
                        .HasConstraintName("fk_teams_families_family_id");

                    b.HasOne("Buk.UniversalGames.Data.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .HasConstraintName("fk_teams_leagues_league_id");

                    b.Navigation("Family");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Buk.UniversalGames.Data.Models.Family", b =>
                {
                    b.Navigation("Teams");
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
