﻿// <auto-generated />
using ColorMixer.Storage.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ColorMixer.Storage.Migrations.SettingsDb
{
    [DbContext(typeof(SettingsDbContext))]
    partial class SettingsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("ColorMixer.Storage.Models.SettingsModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT")
                        .HasColumnName("Key");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT")
                        .HasColumnName("Value");

                    b.HasKey("Key");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            Key = "DARK_MODE",
                            Value = "false"
                        },
                        new
                        {
                            Key = "HIGHT_CONTRAST",
                            Value = "false"
                        },
                        new
                        {
                            Key = "USE_OS_THEME",
                            Value = "true"
                        },
                        new
                        {
                            Key = "SELECTED_THEME",
                            Value = "Blue"
                        },
                        new
                        {
                            Key = "SELECTED_UI_CULTURE"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
