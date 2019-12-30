﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ObjectConfig.Data;

namespace ObjectConfig.Migrations
{
    [DbContext(typeof(ObjectConfigContext))]
    partial class ObjectConfigContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.EntityFrameworkHiLoSequence", "'EntityFrameworkHiLoSequence', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ObjectConfig.Data.Application", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("ApplicationId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("ObjectConfig.Data.Config", b =>
                {
                    b.Property<int>("ConfigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("ConfigElementId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("DateFrom")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateTo")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("int");

                    b.Property<string>("VersionFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(23)")
                        .HasMaxLength(23);

                    b.Property<string>("VersionTo")
                        .HasColumnType("nvarchar(23)")
                        .HasMaxLength(23);

                    b.HasKey("ConfigId");

                    b.HasIndex("ConfigElementId")
                        .IsUnique();

                    b.HasIndex("EnvironmentId");

                    b.HasIndex("Code", "VersionFrom", "EnvironmentId")
                        .IsUnique();

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("ObjectConfig.Data.ConfigElement", b =>
                {
                    b.Property<int>("ConfigElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<int?>("ParrentConfigElementId")
                        .HasColumnType("int");

                    b.Property<int?>("TypeElementId")
                        .HasColumnType("int");

                    b.HasKey("ConfigElementId");

                    b.HasIndex("ParrentConfigElementId");

                    b.HasIndex("TypeElementId");

                    b.ToTable("ConfigElements");
                });

            modelBuilder.Entity("ObjectConfig.Data.Environment", b =>
                {
                    b.Property<int>("EnvironmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("EnvironmentId");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("Code", "ApplicationId")
                        .IsUnique();

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("ObjectConfig.Data.TypeElement", b =>
                {
                    b.Property<int>("TypeElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("TypeElementId");

                    b.ToTable("TypeElements");
                });

            modelBuilder.Entity("ObjectConfig.Data.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("IsGlobalAdmin")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("ExternalId")
                        .IsUnique()
                        .HasFilter("[ExternalId] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            DisplayName = "GlobalAdmin",
                            Email = "admin@global.net",
                            ExternalId = "0701ea11-3386-4bcb-9726-49d7619ea1a5",
                            IsGlobalAdmin = true
                        });
                });

            modelBuilder.Entity("ObjectConfig.Data.UsersApplications", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<int>("AccessRole")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ApplicationId");

                    b.HasIndex("ApplicationId");

                    b.ToTable("UsersApplications");
                });

            modelBuilder.Entity("ObjectConfig.Data.UsersEnvironments", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("int");

                    b.Property<int>("AccessRole")
                        .HasColumnType("int");

                    b.HasKey("UserId", "EnvironmentId");

                    b.HasIndex("EnvironmentId");

                    b.ToTable("UsersEnvironments");
                });

            modelBuilder.Entity("ObjectConfig.Data.UsersTypes", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ValueTypeId")
                        .HasColumnType("int");

                    b.Property<int>("AccessRole")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ValueTypeId");

                    b.HasIndex("ValueTypeId");

                    b.ToTable("UsersTypes");
                });

            modelBuilder.Entity("ObjectConfig.Data.ValueElement", b =>
                {
                    b.Property<int>("ValueElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:HiLoSequenceName", "EntityFrameworkHiLoSequence")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<int?>("ChangeOwnerUserId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(2147483647);

                    b.Property<int?>("ConfigElementId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("DateFrom")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DateTo")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("TypeElementId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(2147483647);

                    b.HasKey("ValueElementId");

                    b.HasIndex("ChangeOwnerUserId");

                    b.HasIndex("ConfigElementId");

                    b.HasIndex("TypeElementId");

                    b.ToTable("ValueElements");
                });

            modelBuilder.Entity("ObjectConfig.Data.Config", b =>
                {
                    b.HasOne("ObjectConfig.Data.ConfigElement", "ConfigElement")
                        .WithOne("Config")
                        .HasForeignKey("ObjectConfig.Data.Config", "ConfigElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ObjectConfig.Data.Environment", "Environment")
                        .WithMany("Configs")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ObjectConfig.Data.ConfigElement", b =>
                {
                    b.HasOne("ObjectConfig.Data.ConfigElement", "Parrent")
                        .WithMany("Childs")
                        .HasForeignKey("ParrentConfigElementId");

                    b.HasOne("ObjectConfig.Data.TypeElement", "Type")
                        .WithMany()
                        .HasForeignKey("TypeElementId");
                });

            modelBuilder.Entity("ObjectConfig.Data.Environment", b =>
                {
                    b.HasOne("ObjectConfig.Data.Application", "Application")
                        .WithMany("Environments")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ObjectConfig.Data.UsersApplications", b =>
                {
                    b.HasOne("ObjectConfig.Data.Application", "Application")
                        .WithMany("Users")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ObjectConfig.Data.User", "User")
                        .WithMany("Applications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ObjectConfig.Data.UsersEnvironments", b =>
                {
                    b.HasOne("ObjectConfig.Data.Environment", "Environment")
                        .WithMany("Users")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ObjectConfig.Data.User", "User")
                        .WithMany("Environments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ObjectConfig.Data.UsersTypes", b =>
                {
                    b.HasOne("ObjectConfig.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ObjectConfig.Data.TypeElement", "ValueType")
                        .WithMany()
                        .HasForeignKey("ValueTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ObjectConfig.Data.ValueElement", b =>
                {
                    b.HasOne("ObjectConfig.Data.User", "ChangeOwner")
                        .WithMany()
                        .HasForeignKey("ChangeOwnerUserId");

                    b.HasOne("ObjectConfig.Data.ConfigElement", null)
                        .WithMany("Value")
                        .HasForeignKey("ConfigElementId");

                    b.HasOne("ObjectConfig.Data.TypeElement", "Type")
                        .WithMany()
                        .HasForeignKey("TypeElementId");
                });
#pragma warning restore 612, 618
        }
    }
}
