using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MopromanWebApi.Models;

public partial class MopromanDbContext : DbContext
{
    public MopromanDbContext()
    {
    }

    public MopromanDbContext(DbContextOptions<MopromanDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Record> Records { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=MopromanCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_records");

            entity.ToTable("record");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("date_time");
            entity.Property(e => e.Frekvencia).HasColumnName("frekvencia");
            entity.Property(e => e.Napatie).HasColumnName("napatie");
            entity.Property(e => e.PecId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("pec_id");
            entity.Property(e => e.PrietokVody).HasColumnName("prietok_vody");
            entity.Property(e => e.Prud).HasColumnName("prud");
            entity.Property(e => e.RzPribenie).HasColumnName("rz_pribenie");
            entity.Property(e => e.SobertVstup).HasColumnName("sobert_vstup");
            entity.Property(e => e.SobertVykon).HasColumnName("sobert_vykon");
            entity.Property(e => e.TVodaVstup).HasColumnName("t_voda_vstup");
            entity.Property(e => e.TVodaVystup).HasColumnName("t_voda_vystup");
            entity.Property(e => e.TeplotaOkruh).HasColumnName("teplota_okruh");
            entity.Property(e => e.TeplotaP1).HasColumnName("teplota_p1");
            entity.Property(e => e.TeplotaP2).HasColumnName("teplota_p2");
            entity.Property(e => e.Tlak).HasColumnName("tlak");
            entity.Property(e => e.Vykon).HasColumnName("vykon");
            //entity.Property(e => e.OkamzitaSpotreba).HasDefaultValue(null);
            entity.Property(e => e.Zmena)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("zmena");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
