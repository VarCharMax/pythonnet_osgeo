using NETPython;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Threading;

namespace gdal
{
  internal class Program
  {
    static void Main(string[] args)
    {
      // string osgeoRoot = @"C:\Users\rpark\AppData\Local\Programs\OSGeo4W";

      string pathToVirtualEnv = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", ".venv");
      string message;

      using PythonInitialiser pyInit = new();
      if ((message = pyInit.InitialisePy(pathToVirtualEnv)) != "")
      {
        Console.WriteLine(message);
        return;
      }

      // Environment.SetEnvironmentVariable("PYTHONPATH", "", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PYTHONUTF8", "1", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("OSGEO4W_ROOT", osgeoRoot, EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pDllPath, EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PYTHONHOME", @$"%OSGEO4W_ROOT%\apps\Python312", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PATH", @$"%OSGEO4W_ROOT%\apps\Python312\Scripts;C:\Users\rpark\AppData\Local\Programs\OSGeo4W\bin;C:\WINDOWS\system32;C:\WINDOWS;C:\WINDOWS\system32\WBem", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("GDAL_DATA", @$"%OSGEO4W_ROOT%\apps\gdal\share\gdal", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("GDAL_DRIVER_PATH", @$"%OSGEO4W_ROOT%\apps\gdal\lib\gdalplugins", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PROJ_DATA", @$"%OSGEO4W_ROOT%\share\proj", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("OPENSSL_ENGINES", @$"%OSGEO4W_ROOT%\lib\engines-3", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("SSL_CERT_FILE", @$"%OSGEO4W_ROOT%\bin\curl-ca-bundle.crt", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("SSL_CERT_DIR", @$"%OSGEO4W_ROOT%\apps\openssl\certs", EnvironmentVariableTarget.Process);

      // Runtime.PythonDLL = pDllPath;
      using (Py.GIL())
      {
        try
        {
          Console.WriteLine("\nDemonstrating the GDAL/OGR libraries:");
          dynamic module = Py.Import("analyze");

          var shpFile = @"C:\shp\tl_2025_us_state.shp";

          module.readshp(shpFile);
          module.readfeature(shpFile, 2);
          module.describegeometry(shpFile, 2);
          module.describepoints(shpFile, 53);

          Console.WriteLine("\nDemonstrating the PROJ4 library:");
          module = Py.Import("proj");
          module.reprojectpoints();

          Console.WriteLine("\nDemonstrating the Shapely library:");
          module = Py.Import("shape");
          module.describeshape();
        }
        catch (PythonException pex)
        {
          Console.WriteLine(pex.Format());
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }

      Console.ReadKey();
    }
  }
}