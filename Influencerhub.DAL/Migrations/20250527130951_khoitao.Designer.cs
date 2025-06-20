﻿// <auto-generated />
using System;
using Influencerhub.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Influencerhub.DAL.Migrations
{
    [DbContext(typeof(InfluencerhubDBContext))]
    [Migration("20250527130951_khoitao")]
    partial class khoitao
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Influencerhub.DAL.Models.Business", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BusinessLicense")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.BusinessField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FieldId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("FieldId");

                    b.ToTable("BusinessFields");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Field", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.FreelanceField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FieldId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FreelanceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("FreelanceId");

                    b.ToTable("FreelanceFields");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.FreelanceJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CancelTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FreelanceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("JobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FreelanceId");

                    b.HasIndex("JobId");

                    b.ToTable("FreelanceJobs");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Influ", b =>
                {
                    b.Property<Guid>("InfluId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Follower")
                        .HasColumnType("int");

                    b.Property<string>("LinkImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Portfolio_link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("InfluId");

                    b.HasIndex("UserId");

                    b.ToTable("Influs");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Budget")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Require")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Link", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("InfluId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Linkmxh")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InfluId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Membership", b =>
                {
                    b.Property<Guid>("BusinessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BusinessId");

                    b.HasIndex("UserId");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.MembershipType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MembershipTypes");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Representative", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Representatives");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FreelanceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("JobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float?>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("feedback")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("FreelanceId");

                    b.HasIndex("JobId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("MembershipId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Time")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MembershipId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpireTimeRefreshToken")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Business", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.BusinessField", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId");

                    b.HasOne("Influencerhub.DAL.Models.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId");

                    b.Navigation("Business");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.FreelanceField", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId");

                    b.HasOne("Influencerhub.DAL.Models.Influ", "Influ")
                        .WithMany()
                        .HasForeignKey("FreelanceId");

                    b.Navigation("Field");

                    b.Navigation("Influ");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.FreelanceJob", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Influ", "Influ")
                        .WithMany()
                        .HasForeignKey("FreelanceId");

                    b.HasOne("Influencerhub.DAL.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId");

                    b.Navigation("Influ");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Influ", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Job", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId");

                    b.Navigation("Business");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Link", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Influ", "Influ")
                        .WithMany()
                        .HasForeignKey("InfluId");

                    b.Navigation("Influ");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Membership", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Representative", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId");

                    b.Navigation("Business");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Review", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId");

                    b.HasOne("Influencerhub.DAL.Models.Influ", "Influ")
                        .WithMany()
                        .HasForeignKey("FreelanceId");

                    b.HasOne("Influencerhub.DAL.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId");

                    b.Navigation("Business");

                    b.Navigation("Influ");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.Transaction", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Membership", "Membership")
                        .WithMany()
                        .HasForeignKey("MembershipId");

                    b.HasOne("Influencerhub.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Membership");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Influencerhub.DAL.Models.User", b =>
                {
                    b.HasOne("Influencerhub.DAL.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
