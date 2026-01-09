# pythonnet_osgeo

This is my implementation of the code from the book _Python Geospatial Development_ by Erik Westra. It's fairly old, but the techniques are still current. It's a book I always meant to read when I was actually doing GIS work, but didn't get around to.

Having had some success with pythonnet, I wanted to see if I could get it to work with the OSGeo4W Python distribution.
The answer is "yes", but I had to use the `virtualenv` manager, which works with arbitrary Python installations.

`virtualenv --python C:\Users\<user>\AppData\Local\Programs\OSGeo4W\apps\Python312\python3.exe .venv`

I modified my Python Initialiser to be compatible with the `virtualenv` library, whcih produces a different set of keys and values to `venv`.

I've added in all the environment variables that the OSGeo4W batch file creates. A better approach would be to parse the batch file and create them using RE. Attempting to run the batch file inside the project didn't really work very well, and overall I don't think this is a very sound approach. I'm not sure how many of the settings are needed. I'll try finding out through a process of elimination.

The main thing I found was that I had to give an absolute file path to the SHP file when invoking it from the console, even though this was not necessary when just running the Python script. Maybe using the Path module will fix this.

OK, turns out _none_ of the environment settings are necessary for reading SHP files(!).

Note: if you want to run the scripts manually, you need to execute the `OSGeo4W.bat` file first in your terminal.

The console app now suports reading SHP files using `gdal.ogr`, reprojecting using `pyproj`, calculations using `shapely`.

Trying next for Mapnik, but it might be a bridge too far, at least on Windows, as Python3 support is not being maintained, apparently.
