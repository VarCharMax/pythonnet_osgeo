"""_summary_"""

import math
import warnings
import osgeo.ogr

warnings.filterwarnings("ignore", category=FutureWarning)


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


def readfeature(shpfile: str, featurenum: int) -> None:
    """_summary_"""

    shapefile = osgeo.ogr.Open(shpfile)

    try:
        assert shapefile is not None
    except AssertionError as e:
        raise RuntimeError("Could not open shapefile: %s" % shpfile) from e

    layer = shapefile.GetLayer(0)
    feature = layer.GetFeature(featurenum)
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


def describegeometry(shpfile: str, featurenum: int):
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
    feature = layer.GetFeature(featurenum)
    geometry = feature.GetGeometryRef()

    analyzeGeometry(geometry)


def findPoints(geometry, results):
    for i in range(geometry.GetPointCount()):
        x, y, z = geometry.GetPoint(i)
        if results["north"] == None or results["north"][1] < y:
            results["north"] = (x, y)
        if results["south"] == None or results["south"][1] > y:
            results["south"] = (x, y)

    for i in range(geometry.GetGeometryCount()):
        findPoints(geometry.GetGeometryRef(i), results)


def describepoints(shpfile: str, featurenum: int) -> None:
    """_summary_

    Args:
        shpfile (str): _description_
        featurenum (int): _description_

    Raises:
        RuntimeError: _description_
    """
    shapefile = osgeo.ogr.Open(shpfile)

    try:
        assert shapefile is not None
    except AssertionError as e:
        raise RuntimeError("Could not open shapefile: %s" % shpfile) from e

    layer = shapefile.GetLayer(0)
    feature = layer.GetFeature(featurenum)
    geometry = feature.GetGeometryRef()
    results = {"north": None, "south": None}
    findPoints(geometry, results)
    print("Northernmost point is (%0.4f, %0.4f)" % results["north"])
    print("Southernmost point is (%0.4f, %0.4f)" % results["south"])

    lat1 = results["north"][1]
    long1 = results["north"][0]
    lat2 = results["south"][1]
    long2 = results["south"][0]
    rLat1 = math.radians(lat1)
    rLong1 = math.radians(long1)
    rLat2 = math.radians(lat2)
    rLong2 = math.radians(long2)
    dLat = rLat2 - rLat1
    dLong = rLong2 - rLong1
    a = (
        math.sin(dLat / 2) ** 2
        + math.cos(rLat1) * math.cos(rLat2) * math.sin(dLong / 2) ** 2
    )
    c = 2 * math.atan2(math.sqrt(a), math.sqrt(1 - a))
    distance = 6371 * c
    print("Great circle distance is %0.0f meters" % distance)


if __name__ == "__main__":
    readshp("C:\\shp\\tl_2025_us_state.shp")
