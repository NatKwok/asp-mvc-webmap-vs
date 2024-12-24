using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace asp_mvc_webmap_vs.Models;

public partial class MilieuxHumide
{
    public int Id { get; set; }

    public MultiPolygon? Geom { get; set; }

    public int? MhId { get; set; }

    public int? MhTypeDv { get; set; }

    public int? ConsClDv { get; set; }

    public double? Superficie { get; set; }
}
