﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VersionManagement;

namespace VersionManagement.Migrations
{
    [DbContext(typeof(VersionManagementContext))]
    partial class VersionManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeploimentHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("ProjetEnvironmentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("VersionId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProjetEnvironmentId");

                    b.HasIndex("VersionId");

                    b.ToTable("DeploimentHistory");
                });

            modelBuilder.Entity("Environment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Environment");
                });

            modelBuilder.Entity("Projet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projet");
                });

            modelBuilder.Entity("ProjetEnvironment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("EnvironmentId")
                        .HasColumnType("bigint");

                    b.Property<long>("ProjetId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("ProjetId");

                    b.ToTable("ProjetEnvironment");
                });

            modelBuilder.Entity("Version", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("ProjetId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("VersionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjetId");

                    b.ToTable("Version");
                });

            modelBuilder.Entity("DeploimentHistory", b =>
                {
                    b.HasOne("ProjetEnvironment", "ProjetEnvironment")
                        .WithMany()
                        .HasForeignKey("ProjetEnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Version", "Version")
                        .WithMany()
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetEnvironment", b =>
                {
                    b.HasOne("Environment", "Environment")
                        .WithMany("ProjetEnvironments")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet", "Projet")
                        .WithMany("Projet_Environments")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Version", b =>
                {
                    b.HasOne("Projet", "Projet")
                        .WithMany("Versions")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
