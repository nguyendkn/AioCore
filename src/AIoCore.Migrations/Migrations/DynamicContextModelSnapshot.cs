﻿// <auto-generated />
using System;
using AioCore.Domain.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AIoCore.Migrations.Migrations
{
    [DbContext(typeof(DynamicContext))]
    partial class DynamicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicDateValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Value")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("DateValues");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("xml");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicFloatValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Value")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("FloatValues");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicGuidValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Value")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("GuidValues");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicIntegerValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("IntegerValues");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicStringValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EntityTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("StringValues");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicDateValue", b =>
                {
                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicEntity", "Entity")
                        .WithMany("DateValues")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicFloatValue", b =>
                {
                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicEntity", "Entity")
                        .WithMany("FloatValues")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicGuidValue", b =>
                {
                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicEntity", "Entity")
                        .WithMany("GuidValues")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicIntegerValue", b =>
                {
                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicEntity", "Entity")
                        .WithMany("IntegerValues")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicStringValue", b =>
                {
                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AioCore.Domain.DynamicAggregate.DynamicEntity", "Entity")
                        .WithMany("StringValues")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("AioCore.Domain.DynamicAggregate.DynamicEntity", b =>
                {
                    b.Navigation("DateValues");

                    b.Navigation("FloatValues");

                    b.Navigation("GuidValues");

                    b.Navigation("IntegerValues");

                    b.Navigation("StringValues");
                });
#pragma warning restore 612, 618
        }
    }
}
