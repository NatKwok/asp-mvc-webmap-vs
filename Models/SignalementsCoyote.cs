using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace asp_mvc_webmap_vs.Models;

public partial class SignalementsCoyote
{
    public int Id { get; set; }

    public Point? Geom { get; set; }

    public int? EntryId { get; set; }

    public int? ObjId { get; set; }

    public DateOnly? DatObs { get; set; }

    public TimeOnly? HrObs { get; set; }

    public double? NbCoyotes { get; set; }

    public string? Alimentation { get; set; }

    public string? StatutAnimal { get; set; }

    public string? Periode { get; set; }

    public string? CompClass { get; set; }

    public double? ComCode { get; set; }

    public double? Cote { get; set; }

    public string? Territoire { get; set; }

    public string? StatutMention { get; set; }

    public string? Provenance { get; set; }

    public string? Verif { get; set; }

    public double? X { get; set; }

    public double? Y { get; set; }

    public double? Lat { get; set; }

    public double? Long { get; set; }
}
