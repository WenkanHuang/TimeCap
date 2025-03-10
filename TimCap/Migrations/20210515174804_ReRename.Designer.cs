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
    [Migration("20210515174804_ReRename")]
    partial class ReRename
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("TimCap.Model.Capsule", b =>
                {
                    b.Property<int>("CapsuleId")
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

                    b.HasKey("CapsuleId");

                    b.ToTable("Capsules");
                });

            modelBuilder.Entity("TimCap.Model.CapsuleDig", b =>
                {
                    b.Property<int>("CapsuleId")
                        .HasColumnType("int");

                    b.Property<string>("UserDig")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DigTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("CapsuleId", "UserDig");

                    b.ToTable("CapsuleDigs");
                });

            modelBuilder.Entity("TimCap.Model.CapsuleDig", b =>
                {
                    b.HasOne("TimCap.Model.Capsule", "Capsule")
                        .WithMany()
                        .HasForeignKey("CapsuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Capsule");
                });
#pragma warning restore 612, 618
        }
    }
}
