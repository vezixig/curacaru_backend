﻿// <auto-generated />
using System;
using Curacaru.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Curacaru.Backend.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("ClearanceType")
                        .HasColumnType("integer");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Costs")
                        .HasColumnType("numeric");

                    b.Property<decimal>("CostsLastYearBudget")
                        .HasColumnType("numeric");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid?>("DeploymentReportId")
                        .HasColumnType("uuid");

                    b.Property<int>("DistanceToCustomer")
                        .HasColumnType("integer");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EmployeeReplacementId")
                        .HasColumnType("uuid");

                    b.Property<bool>("HasBudgetError")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDone")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPlanned")
                        .HasColumnType("boolean");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("SignatureCustomer")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<string>("SignatureEmployee")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<TimeOnly>("TimeEnd")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly>("TimeStart")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DeploymentReportId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeReplacementId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.AssignmentDeclaration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("CustomerFirstName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("CustomerLastName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("CustomerStreet")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("CustomerZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<Guid>("InsuranceId")
                        .HasColumnType("uuid");

                    b.Property<string>("InsuranceName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("InsuranceStreet")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("InsuranceZipCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("InsuredPersonNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Signature")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<string>("SignatureCity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateOnly>("SignatureDate")
                        .HasColumnType("date");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CustomerZipCode");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("InsuranceZipCode");

                    b.HasIndex("CustomerId", "Year")
                        .IsUnique();

                    b.ToTable("AssignmentDeclarations");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CareBenefitAmount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<decimal>("PreventiveCareAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ReliefAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ReliefAmountLastYear")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SelfPayAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SelfPayRaise")
                        .HasColumnType("numeric");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bic")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<decimal>("EmployeeSalary")
                        .HasColumnType("numeric");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("InstitutionCode")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

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
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.HasKey("Id");

                    b.HasIndex("ZipCode");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.ContactForm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<int>("FontSize")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRounded")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId")
                        .IsUnique();

                    b.ToTable("ContactForms");
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DoClearanceCareBenefit")
                        .HasColumnType("boolean");

                    b.Property<bool>("DoClearancePreventiveCare")
                        .HasColumnType("boolean");

                    b.Property<bool>("DoClearanceReliefAmount")
                        .HasColumnType("boolean");

                    b.Property<bool>("DoClearanceSelfPayment")
                        .HasColumnType("boolean");

                    b.Property<string>("EmergencyContactName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("EmergencyContactPhone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("InsuranceId")
                        .HasColumnType("uuid");

                    b.Property<int?>("InsuranceStatus")
                        .HasColumnType("integer");

                    b.Property<string>("InsuredPersonNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Salutation")
                        .HasColumnType("integer");

                    b.Property<int?>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedEmployeeId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("ZipCode");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.DeploymentReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CareLevel")
                        .HasColumnType("integer");

                    b.Property<int>("ClearanceType")
                        .HasColumnType("integer");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<int?>("CustomerInsuranceStatus")
                        .HasColumnType("integer");

                    b.Property<Guid?>("InsuranceId")
                        .HasColumnType("uuid");

                    b.Property<string>("InsuredPersonNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<string>("SignatureCity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("SignatureCustomer")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<DateOnly>("SignatureDate")
                        .HasColumnType("date");

                    b.Property<string>("SignatureEmployee")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<double>("WorkedHours")
                        .HasColumnType("double precision");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("InsuranceId");

                    b.HasIndex("CustomerId", "Year", "Month", "ClearanceType")
                        .IsUnique();

                    b.ToTable("DeploymentReports");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("IsManager")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

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
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ZipCode");

                    b.ToTable("Insurances");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DeploymentReportId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("InvoiceDate")
                        .HasColumnType("date");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("InvoiceTotal")
                        .HasColumnType("numeric");

                    b.Property<decimal>("RideCosts")
                        .HasColumnType("numeric");

                    b.Property<int>("RideCostsType")
                        .HasColumnType("integer");

                    b.Property<string>("Signature")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<Guid>("SignedEmployeeId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalRideCosts")
                        .HasColumnType("numeric");

                    b.Property<decimal>("WorkedHours")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DeploymentReportId")
                        .IsUnique();

                    b.HasIndex("SignedEmployeeId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("PeriodEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("PeriodStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PriceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StripeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SubscriptionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.WorkingTimeReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<string>("SignatureEmployee")
                        .IsRequired()
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<string>("SignatureEmployeeCity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateOnly>("SignatureEmployeeDate")
                        .HasColumnType("date");

                    b.Property<string>("SignatureManager")
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)");

                    b.Property<string>("SignatureManagerCity")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateOnly?>("SignatureManagerDate")
                        .HasColumnType("date");

                    b.Property<double>("TotalHours")
                        .HasColumnType("double precision");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("EmployeeId", "Year", "Month")
                        .IsUnique();

                    b.ToTable("WorkingTimeReports");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.ZipCity", b =>
                {
                    b.Property<string>("ZipCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("ZipCode");

                    b.ToTable("ZipCities");
                });

            modelBuilder.Entity("CustomerProduct", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<int>("ProductsId")
                        .HasColumnType("integer");

                    b.HasKey("CustomerId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("CustomerProduct");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Appointment", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Customer", "Customer")
                        .WithMany("Appointments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.DeploymentReport", null)
                        .WithMany("Appointments")
                        .HasForeignKey("DeploymentReportId");

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

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.AssignmentDeclaration", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Customer", "Customer")
                        .WithMany("AssignmentDeclarations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.ZipCity", "CustomerZipCity")
                        .WithMany()
                        .HasForeignKey("CustomerZipCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Insurance", null)
                        .WithMany()
                        .HasForeignKey("InsuranceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.ZipCity", "InsuranceZipCity")
                        .WithMany()
                        .HasForeignKey("InsuranceZipCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("CustomerZipCity");

                    b.Navigation("InsuranceZipCity");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Budget", b =>
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

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Company", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.ZipCity", "ZipCity")
                        .WithMany()
                        .HasForeignKey("ZipCode");

                    b.Navigation("ZipCity");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.ContactForm", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithOne()
                        .HasForeignKey("Curacaru.Backend.Core.Entities.ContactForm", "CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.DeploymentReport", b =>
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

                    b.HasOne("Curacaru.Backend.Core.Entities.Insurance", "Insurance")
                        .WithMany()
                        .HasForeignKey("InsuranceId");

                    b.Navigation("Customer");

                    b.Navigation("Insurance");
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

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Invoice", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.DeploymentReport", "DeploymentReport")
                        .WithOne("Invoice")
                        .HasForeignKey("Curacaru.Backend.Core.Entities.Invoice", "DeploymentReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Employee", "SignedEmployee")
                        .WithMany()
                        .HasForeignKey("SignedEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeploymentReport");

                    b.Navigation("SignedEmployee");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Subscription", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.WorkingTimeReport", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("Employee");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("CustomerProduct", b =>
                {
                    b.HasOne("Curacaru.Backend.Core.Entities.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Curacaru.Backend.Core.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.Customer", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("AssignmentDeclarations");
                });

            modelBuilder.Entity("Curacaru.Backend.Core.Entities.DeploymentReport", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Invoice");
                });
#pragma warning restore 612, 618
        }
    }
}
