import mapnik

symbolizer = mapnik.PolygonSymbolizer()

symbolizer.fill = mapnik.Color("green")

rule = mapnik.Rule()
rule.symbols.append(symbolizer)

style = mapnik.Style()
style.rules.append(rule)

layer = mapnik.Layer("mapLayer")
layer.datasource = mapnik.Shapefile(file="/mnt/c/shp/world_merc.shp")
layer.styles.append("mapStyle")

map = mapnik.Map(800, 400)
map.background = mapnik.Color("steelblue")
map.append_style("mapStyle", style)
map.layers.append(layer)

map.zoom_all()

mapnik.render_to_file(map, "/mnt/c/tmp/map.png", "png")
