"""
Docstring for mnik1
"""

import mapnik


# 1. Instantiate a map object (width, height, spatial reference system)
m = mapnik.Map(
    600,
    400,
    "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext +no_defs +over",
)

# 2. Set the background color
m.background_color = mapnik.Color("steelblue")

# 3. Load style and data from an XML file
style_path = "/mnt/c/shp/world.xml"
mapnik.load_map(m, style_path)

# 4. Zoom to fit all layers
m.zoom_all()

# 5. Render the map to a PNG file
output_file = "/mnt/c/tmp/world_population.png"
mapnik.render_to_file(m, output_file)

print(f"Map rendered to {output_file}")
