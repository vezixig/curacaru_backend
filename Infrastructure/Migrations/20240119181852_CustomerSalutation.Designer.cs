﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Curacaru.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240119181852_CustomerSalutation")]
    partial class CustomerSalutation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EmployeeReplacementId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDone")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSignedByCustomer")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSignedByEmployee")
                        .HasColumnType("boolean");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeOnly>("TimeEnd")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly>("TimeStart")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeReplacementId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("EmployeeSalary")
                        .HasColumnType("numeric");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InstitutionCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("PricePerHour")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("RecognitionDate")
                        .HasColumnType("date");

                    b.Property<decimal>("RideCosts")
                        .HasColumnType("numeric");

                    b.Property<int>("RideCostsType")
                        .HasColumnType("integer");

                    b.Property<string>("ServiceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ZipCode");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AssociatedEmployeeId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<int>("CareLevel")
                        .HasColumnType("integer");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<List<int>>("DeclarationsOfAssignment")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<bool>("DoClearanceCareBenefit")
                        .HasColumnType("boolean");

                    b.Property<bool>("DoClearanceReliefAmount")
                        .HasColumnType("boolean");

                    b.Property<string>("EmergencyContactName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmergencyContactPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("InsuranceId")
                        .HasColumnType("uuid");

                    b.Property<int?>("InsuranceStatus")
                        .HasColumnType("integer");

                    b.Property<string>("InsuredPersonNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCareContractAvailable")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Salutation")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedEmployeeId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("ZipCode");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsManager")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Insurance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("InstitutionCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ZipCode");

                    b.ToTable("Insurances");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.ZipCity", b =>
                {
                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ZipCode");

                    b.ToTable("ZipCities");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Appointment", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Employee", "EmployeeReplacement")
                        .WithMany()
                        .HasForeignKey("EmployeeReplacementId");

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("EmployeeReplacement");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Company", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.ZipCity", "ZipCity")
                        .WithMany()
                        .HasForeignKey("ZipCode");

                    b.Navigation("ZipCity");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Customer", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Employee", "AssociatedEmployee")
                        .WithMany()
                        .HasForeignKey("AssociatedEmployeeId");

                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Insurance", "Insurance")
                        .WithMany()
                        .HasForeignKey("InsuranceId");

                    b.HasOne("Curacaru.Backend.Core.Entities.ZipCity", "ZipCity")
                        .WithMany()
                        .HasForeignKey("ZipCode");

                    b.Navigation("AssociatedEmployee");

                    b.Navigation("Insurance");

                    b.Navigation("ZipCity");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Employee", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Insurance", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("Curacaru.Backend.Core.Entities.ZipCity", "ZipCity")
                        .WithMany()
                        .HasForeignKey("ZipCode");

                    b.Navigation("ZipCity");
                });
#pragma warning restore 612, 618
        }
    }
}
