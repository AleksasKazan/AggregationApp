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
    [DbContext(typeof(AggregationDbContext))]
    [Migration("20231001143914_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Contracts.Models.AggregatedData", b =>
                {
                    b.Property<string>("Tinklas")
                        .HasColumnType("text");

                    b.Property<decimal?>("PMinusSum")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("PPlusSum")
                        .HasColumnType("numeric");

                    b.Property<int>("TotalRecords")
                        .HasColumnType("integer");

                    b.HasKey("Tinklas");

                    b.ToTable("AggregatedData");
                });
#pragma warning restore 612, 618
        }
    }
}
