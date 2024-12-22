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

        modelBuilder.Entity<MilieuxHumide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milieux_humides_pkey");

            entity.ToTable("milieux_humides");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsClDv).HasColumnName("cons_cl_dv");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Polygon,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.MhId).HasColumnName("mh_id");
            entity.Property(e => e.MhTypeDv).HasColumnName("mh_type_dv");
            entity.Property(e => e.Superficie).HasColumnName("superficie");
        });


        modelBuilder.Entity<SignalementsCoyote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("signalements_coyotes_pkey");

            entity.ToTable("signalements_coyotes");

            entity.HasIndex(e => e.Geom, "sidx_signalements_coyotes_geom").HasMethod("gist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alimentation)
                .HasColumnType("character varying")
                .HasColumnName("alimentation");
            entity.Property(e => e.ComCode).HasColumnName("com_code");
            entity.Property(e => e.CompClass)
                .HasColumnType("character varying")
                .HasColumnName("comp_class");
            entity.Property(e => e.Cote).HasColumnName("cote");
            entity.Property(e => e.DatObs).HasColumnName("dat_obs");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Point,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.HrObs).HasColumnName("hr_obs");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Long).HasColumnName("long");
            entity.Property(e => e.NbCoyotes).HasColumnName("nb_coyotes");
            entity.Property(e => e.ObjId).HasColumnName("obj_id");
            entity.Property(e => e.Periode)
                .HasColumnType("character varying")
                .HasColumnName("periode");
            entity.Property(e => e.Provenance)
                .HasColumnType("character varying")
                .HasColumnName("provenance");
            entity.Property(e => e.StatutAnimal)
                .HasColumnType("character varying")
                .HasColumnName("statut_animal");
            entity.Property(e => e.StatutMention)
                .HasColumnType("character varying")
                .HasColumnName("statut_mention");
            entity.Property(e => e.Territoire)
                .HasColumnType("character varying")
                .HasColumnName("territoire");
            entity.Property(e => e.Verif)
                .HasColumnType("character varying")
                .HasColumnName("verif");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
