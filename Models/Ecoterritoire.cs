using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace asp_mvc_webmap_vs.Models;

public partial class Ecoterritoire
{
    public int Id { get; set; }

    public Polygon? Geom { get; set; }

    public string? Text { get; set; }

    public double? ShapeLeng { get; set; }

    public double? ShapeArea { get; set; }
}
