﻿// <auto-generated />
using System;
using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AioCore.Web.Migrations.Settings
{
    [DbContext(typeof(SettingsContext))]
    [Migration("20220828172725_CreateSaveTypeForSettingCode")]
    partial class CreateSaveTypeForSettingCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Settings")
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.HasSequence<int>("Sequence_D14134D2B12E52CA5E1278F73D12928F");

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AttributeType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR [Settings].Sequence_D14134D2B12E52CA5E1278F73D12928F");

                    b.HasKey("Id");

                    b.HasIndex("Order");

                    b.HasIndex("EntityId", "Name");

                    b.ToTable("Attributes", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PathType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SaveType")
                        .HasColumnType("int");

                    b.Property<Guid?>("TenantId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("TenantId");

                    b.ToTable("Codes", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Entities", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingFeature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Href")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Features", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingForm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("Forms", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingFormAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ColSpan")
                        .HasColumnType("int");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FormId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("FormId");

                    b.ToTable("FormAttributes", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingTenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Keyword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tenants", "Settings");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingAttribute", b =>
                {
                    b.HasOne("AioCore.Domain.SettingAggregate.SettingEntity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingCode", b =>
                {
                    b.HasOne("AioCore.Domain.SettingAggregate.SettingCode", "Parent")
                        .WithMany("Child")
                        .HasForeignKey("ParentId");

                    b.HasOne("AioCore.Domain.SettingAggregate.SettingTenant", "Tenant")
                        .WithMany("Codes")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingForm", b =>
                {
                    b.HasOne("AioCore.Domain.SettingAggregate.SettingEntity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingFormAttribute", b =>
                {
                    b.HasOne("AioCore.Domain.SettingAggregate.SettingAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId");

                    b.HasOne("AioCore.Domain.SettingAggregate.SettingForm", "Form")
                        .WithMany("Attributes")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Form");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingCode", b =>
                {
                    b.Navigation("Child");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingForm", b =>
                {
                    b.Navigation("Attributes");
                });

            modelBuilder.Entity("AioCore.Domain.SettingAggregate.SettingTenant", b =>
                {
                    b.Navigation("Codes");
                });
#pragma warning restore 612, 618
        }
    }
}
