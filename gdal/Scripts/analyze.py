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


def analyzeGeometry(geometry, indent=0) -> None:
    """_summary_

    Args:
        geometry (_type_): _description_
        indent (int, optional): _description_. Defaults to 0.
    """
    s = []
    s.append(" " * indent)
    s.append(geometry.GetGeometryName())
    if geometry.GetPointCount() > 0:
        s.append(" with %d data points" % geometry.GetPointCount())

    if geometry.GetGeometryCount() > 0:
        s.append(" containing:")

    print("".join(s))

    for i in range(geometry.GetGeometryCount()):
        analyzeGeometry(geometry.GetGeometryRef(i), indent + 1)


def describegeometry(shpfile: str, layernum: int):
    """_summary_

    Args:
        shpfile (str): _description_
        layernum (int): _description_

    Raises:
        RuntimeError: _description_
    """
    shapefile = osgeo.ogr.Open(shpfile)

    try:
        assert shapefile is not None
    except AssertionError as e:
        raise RuntimeError("Could not open shapefile: %s" % shpfile) from e

    layer = shapefile.GetLayer(0)
    feature = layer.GetFeature(layernum)
    geometry = feature.GetGeometryRef()

    analyzeGeometry(geometry)


if __name__ == "__main__":
    readshp("C:\\shp\\tl_2025_us_state.shp")
