using System.Collections.Generic;
using System.Text.Json;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;
using Npgsql;
namespace asp_mvc_webmap_vs
{
    public class GeojsonService
    {
        public FeatureCollection FetchGeoJsonFromDatabase()
        {
            var featureCollection = new FeatureCollection();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT id,text_, shape_leng, shape_area, ST_AsGeoJSON(geom) AS geometry FROM ecoterritoires;"; // Replace with your table name

                using var command = new NpgsqlCommand(sqlQuery, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Extract data from the database
                    var id = reader["id"].ToString();
                    var name = reader["text_"].ToString();
                    var geometryJson = reader["geom"].ToString();
                    var ShapeLeng = reader["shape_leng"].ToString();
                    var ShapeArea = reader["shape_area"].ToString();

                    // Parse the geometry into a GeoJSON object
                    var geometry = JsonSerializer.Deserialize<Polygon>(geometryJson);

                    // Create properties dictionary
                    var properties = new Dictionary<string, object>
                        {
                            { "id", id },
                            { "name", name },
                            { "shapeLeng", ShapeLeng },
                            { "shapeArea", ShapeArea }
                        };

                    // Create a GeoJSON Feature
                    var feature = new Feature(geometry, properties);

                    // Add the feature to the FeatureCollection
                    featureCollection.Features.Add(feature);
                }
            }

            return featureCollection;
        }
    
    }
}
