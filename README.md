# pythonnet_osgeo

Having had some success with pythonnet, I wanted to see if I could get it to work with the OSGeo4W python distribution.
The answer is "yes", but not using a virtual environment. First of all, I couldn't use it in combination with my own 3.12 library (which is the current compatibility version) - I had to use the OSGeo4W distribution. Secondly, even with the more flexible `virtualenv` manager, which suppoedly works with arbitrary installations, I couldn't add the OSGeoW python dll in - the attempt just crashed. Thirdly, I couldn't install the `gdal` module in a ve. Installing via the `pip` command crashed with messages about missing headers, libraries, etc. I tried adding in the paths to the C header files and libs, but it only caused more errors. Supposedly it's possible to install it using Anaconda, but I've only recently started using that, and I'll need time to get my head around it.

I've added in all the environment variables that the OSGeoW batch file creates. A better approach would be to parse the batch file and create then using RE. Attempting to run the batch file inside the project didn't really work very well, and overall I don't think this is a very sound approach. I'm not sure how many of the settings are needed. I'll try finding out through a process of elimination.

The main thing I found was that I had to give an absolute file path to the SHP file when invoking it from the console, even though this was not necessary when just running the Python script. Maybe using the Path module will fix this.
