﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerMM;

#nullable disable

namespace ServerMM.Migrations
{
    [DbContext(typeof(SqliteDBContext))]
    [Migration("20241223102348_Database-created")]
    partial class Databasecreated
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("ServerMM.Models.Alert", b =>
                {
                    b.Property<int>("AlertID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AlertMessage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AlertType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAcknowledged")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("AlertID");

                    b.HasIndex("UserID");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("ServerMM.Models.Device", b =>
                {
                    b.Property<int>("DeviceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("DeviceID");

                    b.HasIndex("SerialNumber")
                        .IsUnique();

                    b.HasIndex("UserID");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("ServerMM.Models.Recommendation", b =>
                {
                    b.Property<int>("RecommendationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("GeneratedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecommendationText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecommendationID");

                    b.HasIndex("UserID");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("ServerMM.Models.SensorData", b =>
                {
                    b.Property<int>("DataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActivityLevel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<double?>("BloodOxygenLevel")
                        .HasColumnType("REAL");

                    b.Property<double?>("BodyTemperature")
                        .HasColumnType("REAL");

                    b.Property<int>("DeviceID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HeartRate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SleepPhase")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("DataID");

                    b.HasIndex("DeviceID");

                    b.HasIndex("UserID");

                    b.ToTable("SensorData");
                });

            modelBuilder.Entity("ServerMM.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ServerMM.Models.UserLogin", b =>
                {
                    b.Property<int>("LoginID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoginID");

                    b.HasIndex("UserID");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("ServerMM.Models.Alert", b =>
                {
                    b.HasOne("ServerMM.Models.User", "User")
                        .WithMany("Alerts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerMM.Models.Device", b =>
                {
                    b.HasOne("ServerMM.Models.User", "User")
                        .WithMany("Devices")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerMM.Models.Recommendation", b =>
                {
                    b.HasOne("ServerMM.Models.User", "User")
                        .WithMany("Recommendations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerMM.Models.SensorData", b =>
                {
                    b.HasOne("ServerMM.Models.Device", "Device")
                        .WithMany("SensorData")
                        .HasForeignKey("DeviceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServerMM.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerMM.Models.UserLogin", b =>
                {
                    b.HasOne("ServerMM.Models.User", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerMM.Models.Device", b =>
                {
                    b.Navigation("SensorData");
                });

            modelBuilder.Entity("ServerMM.Models.User", b =>
                {
                    b.Navigation("Alerts");

                    b.Navigation("Devices");

                    b.Navigation("Recommendations");

                    b.Navigation("UserLogins");
                });
#pragma warning restore 612, 618
        }
    }
}