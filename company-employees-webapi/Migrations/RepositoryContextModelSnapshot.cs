﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using company_employees_repositories;

#nullable disable

namespace company_employees_webapi.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("company_employees_entities.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CompanyId");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Address = "Wellington",
                            Country = "New Zealand",
                            Name = "Claudette's Consultancy"
                        },
                        new
                        {
                            Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                            Address = "Wellington",
                            Country = "New Zealand",
                            Name = "Norah's Dolls"
                        });
                });

            modelBuilder.Entity("company_employees_entities.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeId");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("68850305-27ec-4ec4-a2d0-9038662748eb"),
                            Age = 39,
                            CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Name = "Claudette Cabahug",
                            Position = "CEO"
                        },
                        new
                        {
                            Id = new Guid("b239f19a-386a-4004-93ca-db769bd11058"),
                            Age = 6,
                            CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                            Name = "Norah Cabahug",
                            Position = "CEO"
                        });
                });

            modelBuilder.Entity("company_employees_entities.Models.Employee", b =>
                {
                    b.HasOne("company_employees_entities.Models.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("company_employees_entities.Models.Company", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
