﻿// <auto-generated />
using System;
using FastTrackEServices.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FastTrackEServices.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FastTrackEServices.Model.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("contactNumber")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.Property<DateTime>("dateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("FastTrackEServices.Model.OwnedShoe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("clientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("dateAcquired")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("shoeId")
                        .HasColumnType("int");

                    b.Property<int?>("shoeRepairId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("clientId");

                    b.HasIndex("shoeId");

                    b.HasIndex("shoeRepairId");

                    b.ToTable("OwnedShoes");
                });

            modelBuilder.Entity("FastTrackEServices.Model.Shoe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("brand")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Shoes");
                });

            modelBuilder.Entity("FastTrackEServices.Model.ShoeColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("shoeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("shoeId");

                    b.ToTable("ShoeColors");
                });

            modelBuilder.Entity("FastTrackEServices.Model.ShoeRepair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("clientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("dateConfirmed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("dateRegistered")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("clientId");

                    b.ToTable("ShoeRepairs");
                });

            modelBuilder.Entity("FastTrackEServices.Model.OwnedShoe", b =>
                {
                    b.HasOne("FastTrackEServices.Model.Client", "client")
                        .WithMany("ownedShoes")
                        .HasForeignKey("clientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FastTrackEServices.Model.Shoe", "shoe")
                        .WithMany()
                        .HasForeignKey("shoeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FastTrackEServices.Model.ShoeRepair", "shoeRepair")
                        .WithMany("ownedShoes")
                        .HasForeignKey("shoeRepairId");

                    b.Navigation("client");

                    b.Navigation("shoe");

                    b.Navigation("shoeRepair");
                });

            modelBuilder.Entity("FastTrackEServices.Model.ShoeColor", b =>
                {
                    b.HasOne("FastTrackEServices.Model.Shoe", "shoe")
                        .WithMany("shoeColors")
                        .HasForeignKey("shoeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("shoe");
                });

            modelBuilder.Entity("FastTrackEServices.Model.ShoeRepair", b =>
                {
                    b.HasOne("FastTrackEServices.Model.Client", "client")
                        .WithMany("shoeRepairs")
                        .HasForeignKey("clientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("client");
                });

            modelBuilder.Entity("FastTrackEServices.Model.Client", b =>
                {
                    b.Navigation("ownedShoes");

                    b.Navigation("shoeRepairs");
                });

            modelBuilder.Entity("FastTrackEServices.Model.Shoe", b =>
                {
                    b.Navigation("shoeColors");
                });

            modelBuilder.Entity("FastTrackEServices.Model.ShoeRepair", b =>
                {
                    b.Navigation("ownedShoes");
                });
#pragma warning restore 612, 618
        }
    }
}
