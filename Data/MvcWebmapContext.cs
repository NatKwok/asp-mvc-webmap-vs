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

    public virtual DbSet<MilieuxHumide> MilieuxHumides { get; set; }

    public virtual DbSet<SignalementsCoyote> SignalementsCoyotes { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //    => optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=^ytrO524FD;Database=mvc_webmap", x => x.UseNetTopologySuite());

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

        modelBuilder.Entity<MilieuxHumide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milieux-humides_pkey");

            entity.ToTable("milieux-humides");

            entity.HasIndex(e => e.Geom, "sidx_milieux-humides_geom").HasMethod("gist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsClDv).HasColumnName("cons_cl_dv");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(MultiPolygon,32188)")
                .HasColumnName("geom");
            entity.Property(e => e.MhId).HasColumnName("mh_id");
            entity.Property(e => e.MhTypeDv).HasColumnName("mh_type_dv");
            entity.Property(e => e.Superficie).HasColumnName("superficie");
        });

        modelBuilder.Entity<SignalementsCoyote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("signalements-coyotes_pkey");

            entity.ToTable("signalements-coyotes");

            entity.HasIndex(e => e.Geom, "sidx_signalements-coyotes_geom").HasMethod("gist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alimentati)
                .HasMaxLength(80)
                .HasColumnName("alimentati");
            entity.Property(e => e.ComCode).HasColumnName("com_code");
            entity.Property(e => e.CompClass)
                .HasMaxLength(80)
                .HasColumnName("comp_class");
            entity.Property(e => e.Cote).HasColumnName("cote");
            entity.Property(e => e.DatObs)
                .HasMaxLength(80)
                .HasColumnName("dat_obs");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Point,32188)")
                .HasColumnName("geom");
            entity.Property(e => e.HrObs)
                .HasMaxLength(80)
                .HasColumnName("hr_obs");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Long).HasColumnName("long");
            entity.Property(e => e.NbCoyotes).HasColumnName("nb_coyotes");
            entity.Property(e => e.ObjId).HasColumnName("obj_id");
            entity.Property(e => e.Periode)
                .HasMaxLength(80)
                .HasColumnName("periode");
            entity.Property(e => e.Provenance)
                .HasMaxLength(80)
                .HasColumnName("provenance");
            entity.Property(e => e.StatutAni)
                .HasMaxLength(80)
                .HasColumnName("statut_ani");
            entity.Property(e => e.StatutMen)
                .HasMaxLength(80)
                .HasColumnName("statut_men");
            entity.Property(e => e.Territoire)
                .HasMaxLength(80)
                .HasColumnName("territoire");
            entity.Property(e => e.Verif)
                .HasMaxLength(80)
                .HasColumnName("verif");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
