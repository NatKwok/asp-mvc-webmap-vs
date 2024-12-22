using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace asp_mvc_webmap_vs.Models;

public partial class SignalementsCoyote
{
    public int Id { get; set; }

    public Point? Geom { get; set; }

    public long? EntryId { get; set; }

    public long? ObjId { get; set; }

    public string? DatObs { get; set; }

    public string? HrObs { get; set; }

    public decimal? NbCoyotes { get; set; }

    public string? Alimentati { get; set; }

    public string? StatutAni { get; set; }

    public string? Periode { get; set; }

    public string? CompClass { get; set; }

    public decimal? ComCode { get; set; }

    public decimal? Cote { get; set; }

    public string? Territoire { get; set; }

    public string? StatutMen { get; set; }

    public string? Provenance { get; set; }

    public string? Verif { get; set; }

    public decimal? X { get; set; }

    public decimal? Y { get; set; }

    public decimal? Lat { get; set; }

    public decimal? Long { get; set; }
}
