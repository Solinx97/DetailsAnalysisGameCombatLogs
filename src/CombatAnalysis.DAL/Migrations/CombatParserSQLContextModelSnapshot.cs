﻿// <auto-generated />
using System;
using CombatAnalysis.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CombatAnalysis.DAL.Migrations
{
    [DbContext(typeof(CombatParserSQLContext))]
    partial class CombatParserSQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.Combat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CombatLogId")
                        .HasColumnType("int");

                    b.Property<int>("DamageDone")
                        .HasColumnType("int");

                    b.Property<int>("DamageTaken")
                        .HasColumnType("int");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("DungeonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EnergyRecovery")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("FinishDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("HealDone")
                        .HasColumnType("int");

                    b.Property<bool>("IsReady")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWin")
                        .HasColumnType("bit");

                    b.Property<int>("LocallyNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Combat");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.CombatAura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuraCreatorType")
                        .HasColumnType("int");

                    b.Property<int>("AuraType")
                        .HasColumnType("int");

                    b.Property<int>("CombatId")
                        .HasColumnType("int");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("FinishTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Stacks")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CombatAura");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.CombatLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CombatsInQueue")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsReady")
                        .HasColumnType("bit");

                    b.Property<int>("LogType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberReadyCombats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CombatLog");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.CombatPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageItemLevel")
                        .HasColumnType("float");

                    b.Property<int>("CombatId")
                        .HasColumnType("int");

                    b.Property<int>("DamageDone")
                        .HasColumnType("int");

                    b.Property<int>("DamageTaken")
                        .HasColumnType("int");

                    b.Property<int>("HealDone")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResourcesRecovery")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CombatPlayer");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.CombatPlayerPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CombatId")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<double>("PositionX")
                        .HasColumnType("float");

                    b.Property<double>("PositionY")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("CombatPlayerPosition");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.DamageDone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DamageType")
                        .HasColumnType("int");

                    b.Property<bool>("IsPeriodicDamage")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPet")
                        .HasColumnType("bit");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DamageDone");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.DamageDoneGeneral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageValue")
                        .HasColumnType("float");

                    b.Property<int>("CastNumber")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("CritNumber")
                        .HasColumnType("int");

                    b.Property<double>("DamagePerSecond")
                        .HasColumnType("float");

                    b.Property<bool>("IsPet")
                        .HasColumnType("bit");

                    b.Property<int>("MaxValue")
                        .HasColumnType("int");

                    b.Property<int>("MinValue")
                        .HasColumnType("int");

                    b.Property<int>("MissNumber")
                        .HasColumnType("int");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DamageDoneGeneral");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.DamageTaken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Absorbed")
                        .HasColumnType("int");

                    b.Property<int>("ActualValue")
                        .HasColumnType("int");

                    b.Property<int>("Blocked")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DamageTakenType")
                        .HasColumnType("int");

                    b.Property<bool>("IsPeriodicDamage")
                        .HasColumnType("bit");

                    b.Property<int>("Mitigated")
                        .HasColumnType("int");

                    b.Property<int>("RealDamage")
                        .HasColumnType("int");

                    b.Property<int>("Resisted")
                        .HasColumnType("int");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DamageTaken");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.DamageTakenGeneral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActualValue")
                        .HasColumnType("int");

                    b.Property<double>("AverageValue")
                        .HasColumnType("float");

                    b.Property<int>("CastNumber")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("CritNumber")
                        .HasColumnType("int");

                    b.Property<double>("DamageTakenPerSecond")
                        .HasColumnType("float");

                    b.Property<int>("MaxValue")
                        .HasColumnType("int");

                    b.Property<int>("MinValue")
                        .HasColumnType("int");

                    b.Property<int>("MissNumber")
                        .HasColumnType("int");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DamageTakenGeneral");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.HealDone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAbsorbed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCrit")
                        .HasColumnType("bit");

                    b.Property<int>("Overheal")
                        .HasColumnType("int");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("HealDone");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.HealDoneGeneral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageValue")
                        .HasColumnType("float");

                    b.Property<int>("CastNumber")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("CritNumber")
                        .HasColumnType("int");

                    b.Property<double>("HealPerSecond")
                        .HasColumnType("float");

                    b.Property<int>("MaxValue")
                        .HasColumnType("int");

                    b.Property<int>("MinValue")
                        .HasColumnType("int");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("HealDoneGeneral");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.PlayerDeath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<string>("LastHitSpellOrItem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LastHitValue")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlayerDeath");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.PlayerParseInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BossId")
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("DamageEfficiency")
                        .HasColumnType("int");

                    b.Property<int>("Difficult")
                        .HasColumnType("int");

                    b.Property<int>("HealEfficiency")
                        .HasColumnType("int");

                    b.Property<int>("SpecId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PlayerParseInfo");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.ResourceRecovery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ResourceRecovery");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.ResourceRecoveryGeneral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageValue")
                        .HasColumnType("float");

                    b.Property<int>("CastNumber")
                        .HasColumnType("int");

                    b.Property<int>("CombatPlayerId")
                        .HasColumnType("int");

                    b.Property<int>("MaxValue")
                        .HasColumnType("int");

                    b.Property<int>("MinValue")
                        .HasColumnType("int");

                    b.Property<double>("ResourcePerSecond")
                        .HasColumnType("float");

                    b.Property<string>("Spell")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ResourceRecoveryGeneral");
                });

            modelBuilder.Entity("CombatAnalysis.DAL.Entities.SpecializationScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BossId")
                        .HasColumnType("int");

                    b.Property<int>("Damage")
                        .HasColumnType("int");

                    b.Property<int>("Difficult")
                        .HasColumnType("int");

                    b.Property<int>("Heal")
                        .HasColumnType("int");

                    b.Property<int>("SpecId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("SpecializationScore");
                });
#pragma warning restore 612, 618
        }
    }
}
