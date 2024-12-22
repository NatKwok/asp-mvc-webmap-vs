# asp-mvc-webmap-vs
# ASP.NET MVC WebMap Application

## Overview
This project is an ASP.NET MVC web application that integrates a web map with geospatial data using GeoJSON. The application leverages PostgreSQL with PostGIS for spatial data storage and manipulation, and displays spatial data dynamically on an interactive map using Leaflet.js.

### Features
- **Interactive Web Map**: Display geospatial data using Leaflet.js.
- **GeoJSON Integration**: Serve geospatial data as GeoJSON via ASP.NET API endpoints.
- **PostgreSQL + PostGIS**: Utilize PostGIS for storing and querying geospatial data.
- **Entity Framework Core**: Manage database access with EF Core, including spatial data handling via NetTopologySuite.
- **Custom API Endpoints**: Expose data dynamically for frontend consumption.

## Prerequisites
1. **Development Tools**:
   - Visual Studio 2022 or later
   - .NET 6.0 SDK or later

2. **Database**:
   - PostgreSQL with PostGIS extension installed

3. **Frontend Libraries**:
   - Leaflet.js (included in the project)

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/NatKwok/asp-mvc-webmap-vs.git
cd asp-mvc-webmap-vs
git checkout feature/nathan
```

### 2. Configure the Database
- Ensure PostgreSQL with PostGIS is installed.
- Create a database and enable the PostGIS extension:
  ```sql
  CREATE DATABASE webmap_db;
  
  \c webmap_db
  CREATE EXTENSION postgis;
  ```
- Update the connection string in `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=webmap_db;Username=your_user;Password=your_password"
  }
  ```
- Apply migrations to set up the database schema:
  ```bash
  dotnet ef database update
  ```

### 3. Install Dependencies
Restore NuGet packages:
```bash
dotnet restore
```

### 4. Run the Application
Launch the application via Visual Studio or the CLI:
```bash
dotnet run
```

Navigate to `http://localhost:5000` in your web browser.

## Project Structure

- **Controllers**:
  - `EcoterritoireController`: Manages API endpoints for geospatial data.

- **Models**:
  - `Ecoterritoire`: Represents geospatial data entities, including geometry fields.

- **Views**:
  - Razor views for rendering HTML and JavaScript.

- **wwwroot/js**:
  - Contains JavaScript code, including Leaflet.js integrations.

## Key Endpoints

### `/Ecoterritoire/GetEcoterritoires`
- Returns GeoJSON data for ecoterritoires.
- Example response:
  ```json
  {
    "type": "FeatureCollection",
    "features": [
      {
        "type": "Feature",
        "geometry": {
          "type": "Polygon",
          "coordinates": [
            [
              [-73.61454437380193, 45.63201569343023],
              [-73.6136884835177, 45.63115049571408],
              ...
            ]
          ]
        },
        "properties": {
          "Id": 1,
          "Description": "Sample description",
          "Area": 123.45
        }
      }
    ]
  }
  ```

## Development Notes

### GeoJSON Integration
- **NetTopologySuite** is used to handle geospatial data in .NET.
- Geometries are serialized to GeoJSON format and served via API endpoints.

### Leaflet.js Integration
- The frontend uses Leaflet.js to render GeoJSON data dynamically on the map.
- JavaScript code manages the AJAX calls and map interactions.

### Troubleshooting
1. **Database Connection Issues**:
   - Verify the connection string in `appsettings.json`.
   - Ensure PostgreSQL and PostGIS are installed and running.

2. **GeoJSON Rendering Issues**:
   - Check the structure of the GeoJSON data returned by the API.
   - Verify that geometries have the correct `type` and `coordinates`.

## License
This project is licensed under the MIT License. See the LICENSE file for details.

## Contact
For questions or suggestions, please contact the repository owner or raise an issue in the GitHub repository.

