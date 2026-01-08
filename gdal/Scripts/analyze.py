"""_summary_"""

import osgeo.ogr


def readshp(shpfile: str) -> None:
    """_summary_"""
    shapefile = osgeo.ogr.Open(shpfile)

    try:
        assert shapefile is not None
    except AssertionError as e:
        raise RuntimeError("Could not open shapefile: %s" % shpfile) from e

    numLayers = shapefile.GetLayerCount()

    print("Shapefile contains %d layers" % numLayers)

    for layerNum in range(numLayers):
        layer = shapefile.GetLayer(layerNum)
        spatialRef = layer.GetSpatialRef().ExportToProj4()
        numFeatures = layer.GetFeatureCount()
        print("Layer %d has spatial reference %s" % (layerNum, spatialRef))
        print("Layer %d has %d features:" % (layerNum, numFeatures))
        for featureNum in range(numFeatures):
            feature = layer.GetFeature(featureNum)
            featureName = feature.GetField("NAME")
            print("Feature %d has name %s" % (featureNum, featureName))

    shapefile = None


def readfeature(shpfile: str, layernum: int) -> None:
    """_summary_"""

    shapefile = osgeo.ogr.Open(shpfile)

    try:
        assert shapefile is not None
    except AssertionError as e:
        raise RuntimeError("Could not open shapefile: %s" % shpfile) from e

    layer = shapefile.GetLayer(0)
    feature = layer.GetFeature(layernum)
    print("Feature 2 has the following attributes:")
    attributes = feature.items()
    for key, value in attributes.items():
        print(" %s = %s" % (key, value))

    geometry = feature.GetGeometryRef()
    geometryName = geometry.GetGeometryName()
    print("Feature's geometry data consists of a %s" % geometryName)


if __name__ == "__main__":
    readshp("C:\\shp\\tl_2025_us_state.shp")
