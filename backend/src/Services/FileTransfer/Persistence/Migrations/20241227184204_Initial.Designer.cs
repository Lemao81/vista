﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(FileTransferDbContext))]
    [Migration("20241227184204_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("filetransfer")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Media.MediaFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<DateTime?>("ModifiedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_utc");

                    b.Property<byte>("StorageVersion")
                        .HasColumnType("smallint")
                        .HasColumnName("storage_version");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_media_folders");

                    b.ToTable("media_folders", "filetransfer");
                });

            modelBuilder.Entity("Domain.Media.MediaItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_utc");

                    b.Property<Guid>("MediaFolderId")
                        .HasColumnType("uuid")
                        .HasColumnName("media_folder_id");

                    b.Property<int>("MediaKind")
                        .HasColumnType("integer")
                        .HasColumnName("media_kind");

                    b.Property<int>("MediaSizeKind")
                        .HasColumnType("integer")
                        .HasColumnName("media_size_kind");

                    b.Property<DateTime?>("ModifiedUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_utc");

                    b.Property<byte>("StorageVersion")
                        .HasColumnType("smallint")
                        .HasColumnName("storage_version");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_media_items");

                    b.HasIndex("MediaFolderId")
                        .HasDatabaseName("ix_media_items_media_folder_id");

                    b.ToTable("media_items", "filetransfer");
                });

            modelBuilder.Entity("Domain.Media.MediaItem", b =>
                {
                    b.HasOne("Domain.Media.MediaFolder", null)
                        .WithMany("MediaItems")
                        .HasForeignKey("MediaFolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_media_items_media_folders_media_folder_id");
                });

            modelBuilder.Entity("Domain.Media.MediaFolder", b =>
                {
                    b.Navigation("MediaItems");
                });
#pragma warning restore 612, 618
        }
    }
}
