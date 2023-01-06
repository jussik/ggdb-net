﻿// <auto-generated />
using System;
using GgdbNet.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GgdbNet.Server.Migrations
{
    [DbContext(typeof(GgdbContext))]
    partial class GgdbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("GgdbNet.Shared.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PublicId")
                        .IsUnique();

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("GgdbNet.Shared.Models.Game", b =>
                {
                    b.Property<int>("CollectionId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AllTitles")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DateAdded")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Platforms")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReleaseIds")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Screenshots")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SortingTitle")
                        .HasColumnType("TEXT");

                    b.Property<long?>("SteamAppId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Themes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("VerticalCover")
                        .HasColumnType("TEXT");

                    b.HasKey("CollectionId", "GameId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GgdbNet.Shared.Models.Game", b =>
                {
                    b.HasOne("GgdbNet.Shared.Models.Collection", null)
                        .WithMany("Games")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GgdbNet.Shared.Models.Collection", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
