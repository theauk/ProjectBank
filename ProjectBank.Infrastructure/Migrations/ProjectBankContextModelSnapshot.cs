﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectBank.Infrastructure;

#nullable disable

namespace ProjectBank.Infrastructure.Migrations
{
    [DbContext(typeof(ProjectBankContext))]
    partial class ProjectBankContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("UniversityDomainName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UniversityDomainName");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("TagGroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("TagGroupId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.TagGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("RequiredInProject")
                        .HasColumnType("boolean");

                    b.Property<bool>("SupervisorCanAddTag")
                        .HasColumnType("boolean");

                    b.Property<int?>("TagLimit")
                        .HasColumnType("integer");

                    b.Property<string>("UniversityDomainName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UniversityDomainName");

                    b.ToTable("TagGroups");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.University", b =>
                {
                    b.Property<string>("DomainName")
                        .HasColumnType("text");

                    b.HasKey("DomainName");

                    b.HasIndex("DomainName")
                        .IsUnique();

                    b.ToTable("Universities");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UniversityDomainName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UniversityDomainName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectTag", b =>
                {
                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("ProjectsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("ProjectTag");
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.Property<int>("SupervisorsId")
                        .HasColumnType("integer");

                    b.HasKey("ProjectsId", "SupervisorsId");

                    b.HasIndex("SupervisorsId");

                    b.ToTable("ProjectUser");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.Project", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Entities.University", "University")
                        .WithMany("Projects")
                        .HasForeignKey("UniversityDomainName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.Tag", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Entities.TagGroup", "TagGroup")
                        .WithMany("Tags")
                        .HasForeignKey("TagGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TagGroup");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.TagGroup", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Entities.University", "University")
                        .WithMany("TagGroups")
                        .HasForeignKey("UniversityDomainName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.User", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Entities.University", "University")
                        .WithMany("Users")
                        .HasForeignKey("UniversityDomainName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("ProjectTag", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("SupervisorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.TagGroup", b =>
                {
                    b.Navigation("Tags");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Entities.University", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("TagGroups");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
