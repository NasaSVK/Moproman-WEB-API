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
        => optionsBuilder.UseSqlServer("name=MopromanCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("record_pk");

            entity.ToTable("record");

            entity.HasIndex(e => e.Id, "record_id_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("(((1999)-(1))-(1))")
                .HasColumnType("datetime")
                .HasColumnName("date_time");
            entity.Property(e => e.Napatie).HasColumnName("napatie");
            entity.Property(e => e.PecId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("pec_id");
            entity.Property(e => e.Prud).HasColumnName("prud");
            entity.Property(e => e.RzPribenie)
                .HasComment("Rz-Prisposobenie")
                .HasColumnName("rz_pribenie");
            entity.Property(e => e.SobertVstup).HasColumnName("sobert_vstup");
            entity.Property(e => e.SobertVykon).HasColumnName("sobert_vykon");
            entity.Property(e => e.TVodaVstup).HasColumnName("t_voda_vstup");
            entity.Property(e => e.TVodaVystup).HasColumnName("t_voda_vystup");
            entity.Property(e => e.Tlak).HasColumnName("tlak");
            entity.Property(e => e.Vykon).HasColumnName("vykon");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
