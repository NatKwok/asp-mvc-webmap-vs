using GeoJSON.Text;
using System.Text.Json;
using GeoJSON.Text.Feature;
using Newtonsoft.Json;
using GeoJSON.Text.Geometry;

namespace asp_mvc_webmap_vs
{
    public class GeojsonConverter()
    {
        public static string CreateFeatureCollection(List<Feature> features)
        {
            var featureCollection = new FeatureCollection(features);
            return JsonConvert.SerializeObject(featureCollection);
        }

        public static Feature CreatePointFeature(double latitude, double longitude, Dictionary<string, object> properties)
        {
            var point = new Point(new Position(latitude, longitude));
            return new Feature(point, properties);
        }

        public static Feature CreateLineStringFeature(List<Position> coordinates, Dictionary<string, object> properties)
        {
            var lineString = new LineString(coordinates);
            return new Feature(lineString, properties);
        }

        public static Feature CreatePolygonFeature(List<LineString> coordinates, Dictionary<string, object> properties)
        {
            var polygon = new Polygon(coordinates);
            return new Feature(polygon, properties);
        }
    }
}
