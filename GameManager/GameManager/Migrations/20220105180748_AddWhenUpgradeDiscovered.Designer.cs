﻿// <auto-generated />
using System;
using GameManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameManager.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220105180748_AddWhenUpgradeDiscovered")]
    partial class AddWhenUpgradeDiscovered
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("GameManager.Data.GameMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Developer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prefixes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("WhenRefreshed")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GameMetadata");
                });

            modelBuilder.Entity("GameManager.Data.LibraryGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvailableUpgradeVersion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Developer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExecutablePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasUpgradeAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MetadataId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prefixes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("WhenUpdated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("WhenUpgradeDiscovered")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Library");
                });
#pragma warning restore 612, 618
        }
    }
}
