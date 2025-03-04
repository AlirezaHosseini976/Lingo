﻿// <auto-generated />
using System;
using Lingo_VerticalSlice.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lingo_VerticalSlice.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250211081506_DictionaryMigration")]
    partial class DictionaryMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.AnonymousEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnonymousEmail");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.CardSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("CardSet");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.CardSetWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardSetId")
                        .HasColumnType("int");

                    b.Property<int?>("VocabularyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardSetId");

                    b.HasIndex("VocabularyId");

                    b.ToTable("CardSetWord");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lesson");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.LessonDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VocabularyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LessonDefinition");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.SpacedRepetition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("NextDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("VocabularyId")
                        .HasColumnType("int");

                    b.Property<int>("WordState")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VocabularyId");

                    b.ToTable("SpacedRepetition");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.UnitWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.Property<int>("VocabularyId")
                        .HasColumnType("int");

                    b.Property<int>("WordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UnitId");

                    b.HasIndex("WordId");

                    b.ToTable("UnitWord");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.VocabularyDetailsMaterializedView", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("uniqueid");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UniqueId"));

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("definition");

                    b.Property<string>("Example")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("examples");

                    b.Property<string>("PartOfSpeech")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("partofspeech");

                    b.Property<string>("Synonym")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("synonyms");

                    b.Property<string>("Translation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vocabulary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VocabularyId")
                        .HasColumnType("int")
                        .HasColumnName("vocabularyid");

                    b.HasKey("UniqueId");

                    b.ToTable("VocabularyDetailsMaterializedView");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Vocabulary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Word");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DefinitionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VocabularyId")
                        .HasColumnType("int");

                    b.Property<int>("WordTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("WordTypeId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VocabularyId");

                    b.HasIndex("WordTypeId");

                    b.HasIndex("WordTypeId1");

                    b.ToTable("WordDefinition");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordDefinitionExample", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DefinitionId")
                        .HasColumnType("int");

                    b.Property<string>("Example")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Proficiency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DefinitionId");

                    b.ToTable("WordDefinitionExample");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordMeaning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DefinitionId")
                        .HasColumnType("int");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DefinitionId");

                    b.ToTable("WordMeaning");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordSynonym", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DefinitionId")
                        .HasColumnType("int");

                    b.Property<string>("Synonym")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DefinitionId");

                    b.ToTable("WordSynonym");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WordType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.CardSet", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.Folder", "Folder")
                        .WithMany("CardSets")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.CardSetWord", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.CardSet", "CardSet")
                        .WithMany("CardSetWords")
                        .HasForeignKey("CardSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lingo_VerticalSlice.Entities.Word", "Words")
                        .WithMany("CardSetWords")
                        .HasForeignKey("VocabularyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CardSet");

                    b.Navigation("Words");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Folder", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.SpacedRepetition", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lingo_VerticalSlice.Entities.Word", "Words")
                        .WithMany("SpacedRepetitions")
                        .HasForeignKey("VocabularyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");

                    b.Navigation("Words");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Unit", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.UnitWord", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lingo_VerticalSlice.Entities.Word", "Word")
                        .WithMany()
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Unit");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordDefinition", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.Word", "Word")
                        .WithMany("WordDefinitions")
                        .HasForeignKey("VocabularyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lingo_VerticalSlice.Entities.WordType", "WordType")
                        .WithMany()
                        .HasForeignKey("WordTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lingo_VerticalSlice.Entities.WordType", null)
                        .WithMany("WordDefinitions")
                        .HasForeignKey("WordTypeId1");

                    b.Navigation("Word");

                    b.Navigation("WordType");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordDefinitionExample", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.WordDefinition", "WordDefinition")
                        .WithMany("WordDefinitionExamples")
                        .HasForeignKey("DefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordDefinition");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordMeaning", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.WordDefinition", "WordDefinition")
                        .WithMany("WordMeaning")
                        .HasForeignKey("DefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordDefinition");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordSynonym", b =>
                {
                    b.HasOne("Lingo_VerticalSlice.Entities.WordDefinition", "WordDefinition")
                        .WithMany("Synonyms")
                        .HasForeignKey("DefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordDefinition");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.CardSet", b =>
                {
                    b.Navigation("CardSetWords");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Folder", b =>
                {
                    b.Navigation("CardSets");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.Word", b =>
                {
                    b.Navigation("CardSetWords");

                    b.Navigation("SpacedRepetitions");

                    b.Navigation("WordDefinitions");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordDefinition", b =>
                {
                    b.Navigation("Synonyms");

                    b.Navigation("WordDefinitionExamples");

                    b.Navigation("WordMeaning");
                });

            modelBuilder.Entity("Lingo_VerticalSlice.Entities.WordType", b =>
                {
                    b.Navigation("WordDefinitions");
                });
#pragma warning restore 612, 618
        }
    }
}
