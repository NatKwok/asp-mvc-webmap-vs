using System;
using System.Collections.Generic;
using asp_mvc_webmap_vs.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_mvc_webmap_vs.Data;

public partial class MvcWebmapContext : DbContext
{
    public MvcWebmapContext()
    {
    }

    public MvcWebmapContext(DbContextOptions<MvcWebmapContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ecoterritoire> Ecoterritoires { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=^ytrO524FD;Database=mvc_webmap", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<Ecoterritoire>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ecoterritoires_pkey");

            entity.ToTable("ecoterritoires");

            entity.HasIndex(e => e.Geom, "sidx_ecoterritoires_geom").HasMethod("gist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Polygon,4030)")
                .HasColumnName("geom");
            entity.Property(e => e.ShapeArea).HasColumnName("shape_area");
            entity.Property(e => e.ShapeLeng).HasColumnName("shape_leng");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text_");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
