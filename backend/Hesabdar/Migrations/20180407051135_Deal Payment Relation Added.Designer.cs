﻿// <auto-generated />
using Hesabdar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Hesabdar.Migrations
{
    [DbContext(typeof(HesabdarContext))]
    [Migration("20180407051135_Deal Payment Relation Added")]
    partial class DealPaymentRelationAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hesabdar.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("Hesabdar.Models.Deal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuyerId");

                    b.Property<DateTime>("DealTime")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int?>("PaymentId");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(15, 3)");

                    b.Property<int>("SellerId");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("SellerId");

                    b.ToTable("Deal");
                });

            modelBuilder.Entity("Hesabdar.Models.Dealer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Dealer");
                });

            modelBuilder.Entity("Hesabdar.Models.DealItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DealId");

                    b.Property<int>("MaterialId");

                    b.Property<decimal>("PricePerOne")
                        .HasColumnType("decimal(15, 3)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(15, 3)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("DealId");

                    b.HasIndex("MaterialId");

                    b.ToTable("DealItem");
                });

            modelBuilder.Entity("Hesabdar.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Material");
                });

            modelBuilder.Entity("Hesabdar.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(15, 3)");

                    b.Property<int>("PayeeId");

                    b.Property<int>("PayerId");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("PayeeId");

                    b.HasIndex("PayerId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("Hesabdar.Models.Deal", b =>
                {
                    b.HasOne("Hesabdar.Models.Dealer", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Hesabdar.Models.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId");

                    b.HasOne("Hesabdar.Models.Dealer", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Hesabdar.Models.DealItem", b =>
                {
                    b.HasOne("Hesabdar.Models.Deal", "Deal")
                        .WithMany("Items")
                        .HasForeignKey("DealId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Hesabdar.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Hesabdar.Models.Payment", b =>
                {
                    b.HasOne("Hesabdar.Models.Dealer", "Payee")
                        .WithMany()
                        .HasForeignKey("PayeeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Hesabdar.Models.Dealer", "Payer")
                        .WithMany()
                        .HasForeignKey("PayerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}