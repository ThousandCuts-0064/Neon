﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neon.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Neon.Data.Migrations
{
    [DbContext(typeof(NeonDbContext))]
    partial class NeonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Neon.Domain.Entities.Friendship", b =>
                {
                    b.Property<int>("User1Id")
                        .HasColumnType("integer");

                    b.Property<int>("User2Id")
                        .HasColumnType("integer");

                    b.HasKey("User1Id", "User2Id");

                    b.ToTable("Friendships", t =>
                        {
                            t.HasCheckConstraint("CK_Friendship_UserId1_LessThan_UserId2", "\"User1Id\" < \"User2Id\"");
                        });
                });

            modelBuilder.Entity("Neon.Domain.Entities.SystemValue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SystemValues");
                });

            modelBuilder.Entity("Neon.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<Guid>("IdentityKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Key")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastActiveAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<Guid>("SecurityKey")
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Neon.Domain.Entities.UserRequests.DuelRequest", b =>
                {
                    b.Property<int>("RequesterId")
                        .HasColumnType("integer");

                    b.Property<int>("ResponderId")
                        .HasColumnType("integer");

                    b.HasKey("RequesterId", "ResponderId");

                    b.ToTable("DuelRequests");
                });

            modelBuilder.Entity("Neon.Domain.Entities.UserRequests.FriendRequest", b =>
                {
                    b.Property<int>("RequesterId")
                        .HasColumnType("integer");

                    b.Property<int>("ResponderId")
                        .HasColumnType("integer");

                    b.HasKey("RequesterId", "ResponderId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("Neon.Domain.Entities.UserRequests.TradeRequest", b =>
                {
                    b.Property<int>("RequesterId")
                        .HasColumnType("integer");

                    b.Property<int>("ResponderId")
                        .HasColumnType("integer");

                    b.HasKey("RequesterId", "ResponderId");

                    b.ToTable("TradeRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
