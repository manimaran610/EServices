﻿// <auto-generated />
using System;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.CustomerDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AreaOfTest")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Client")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfTest")
                        .HasColumnType("datetime2");

                    b.Property<string>("EquipmentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FormType")
                        .HasColumnType("int");

                    b.Property<int>("InstrumentId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("plant")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InstrumentId");

                    b.ToTable("CustomerDetails");
                });

            modelBuilder.Entity("Domain.Entities.Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CalibratedDueOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CalibratedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CertificateFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CertificateName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Make")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Instruments");
                });

            modelBuilder.Entity("Domain.Entities.CustomerDetails", b =>
                {
                    b.HasOne("Domain.Entities.Instrument", "Instrument")
                        .WithMany("CustomerDetails")
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instrument");
                });

            modelBuilder.Entity("Domain.Entities.Instrument", b =>
                {
                    b.Navigation("CustomerDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
