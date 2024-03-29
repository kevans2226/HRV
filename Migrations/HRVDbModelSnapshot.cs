﻿// <auto-generated />
using System;
using HRV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRV.Migrations
{
    [DbContext(typeof(HRVDb))]
    partial class HRVDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("HRV.Data.Db.HRVRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("HRV")
                        .HasColumnType("int");

                    b.Property<string>("UserSid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("HRVs");
                });

            modelBuilder.Entity("HRV.Data.Db.Range", b =>
                {
                    b.Property<int>("RangeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("High")
                        .HasColumnType("int");

                    b.Property<int>("Low")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("RangeId");

                    b.ToTable("Ranges");
                });
#pragma warning restore 612, 618
        }
    }
}
