﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTrackerApp.Data;

namespace TimeTrackerApp.Data.Migrations
{
    [DbContext(typeof(TimeTrackerDbContext))]
    partial class TimeTrackerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview6.19304.10");

            modelBuilder.Entity("TimeTrackerApp.Domain.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Client 1"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Client 2"
                        });
                });

            modelBuilder.Entity("TimeTrackerApp.Domain.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ClientId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ClientId = 1L,
                            Name = "Project 1"
                        },
                        new
                        {
                            Id = 2L,
                            ClientId = 1L,
                            Name = "Project 2"
                        },
                        new
                        {
                            Id = 3L,
                            ClientId = 2L,
                            Name = "Project 3"
                        });
                });

            modelBuilder.Entity("TimeTrackerApp.Domain.TimeEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime>("EntryDate");

                    b.Property<decimal>("HourRate");

                    b.Property<int>("Hours");

                    b.Property<long>("ProjectId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("TimeEntries");
                });

            modelBuilder.Entity("TimeTrackerApp.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("HourRate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            HourRate = 15m,
                            Name = "Joni"
                        },
                        new
                        {
                            Id = 2L,
                            HourRate = 20m,
                            Name = "Bravo"
                        });
                });

            modelBuilder.Entity("TimeTrackerApp.Domain.Project", b =>
                {
                    b.HasOne("TimeTrackerApp.Domain.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TimeTrackerApp.Domain.TimeEntry", b =>
                {
                    b.HasOne("TimeTrackerApp.Domain.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimeTrackerApp.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
