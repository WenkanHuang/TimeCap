﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimCap.DAO;

namespace TimCap.Migrations
{
    [DbContext(typeof(TimeCapContext))]
    [Migration("20210515090801_Rename")]
    partial class Rename
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("TimCap.Model.CapDig", b =>
                {
                    b.Property<int>("CapId")
                        .HasColumnType("int");

                    b.Property<string>("UserDig")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("CapOwnId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DigTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("CapId", "UserDig");

                    b.HasIndex("CapOwnId");

                    b.ToTable("CapDigs");
                });

            modelBuilder.Entity("TimCap.Model.Caps", b =>
                {
                    b.Property<int>("CapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("InTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Story")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("CapId");

                    b.ToTable("Caps");
                });

            modelBuilder.Entity("TimCap.Model.CapDig", b =>
                {
                    b.HasOne("TimCap.Model.Caps", "Cap")
                        .WithMany()
                        .HasForeignKey("CapOwnId");

                    b.Navigation("Cap");
                });
#pragma warning restore 612, 618
        }
    }
}
