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
      string osgeoRoot = @"C:\Users\rpark\AppData\Local\Programs\OSGeo4W";

      string runtime = "python312.dll";
      string pDllPath = @$"{osgeoRoot}\apps\Python312\{runtime}";

      Environment.SetEnvironmentVariable("PYTHONPATH", "", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PYTHONUTF8", "1", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("OSGEO4W_ROOT", osgeoRoot, EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pDllPath, EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PYTHONHOME", @$"%OSGEO4W_ROOT%\apps\Python312", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PATH", @$"%OSGEO4W_ROOT%\apps\Python312\Scripts;C:\Users\rpark\AppData\Local\Programs\OSGeo4W\bin;C:\WINDOWS\system32;C:\WINDOWS;C:\WINDOWS\system32\WBem", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("GDAL_DATA", @$"%OSGEO4W_ROOT%\apps\gdal\share\gdal", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("GDAL_DRIVER_PATH", @$"%OSGEO4W_ROOT%\apps\gdal\lib\gdalplugins", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("PROJ_DATA", @$"%OSGEO4W_ROOT%\share\proj", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("OPENSSL_ENGINES", @$"%OSGEO4W_ROOT%\lib\engines-3", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("SSL_CERT_FILE", @$"%OSGEO4W_ROOT%\bin\curl-ca-bundle.crt", EnvironmentVariableTarget.Process);
      // Environment.SetEnvironmentVariable("SSL_CERT_DIR", @$"%OSGEO4W_ROOT%\apps\openssl\certs", EnvironmentVariableTarget.Process);

      // Runtime.PythonDLL = pDllPath;

      try
      {
        if (!PythonEngine.IsInitialized)
        {
          PythonEngine.Initialize();
        }
      }
      catch (TypeInitializationException tie)
      {
        if (tie.InnerException is DllNotFoundException)
        {
          return; // "The specified Python DLL was not found. Please ensure that the correct version of Python is installed and configured.";
        }
        else
        {
          return; // tie.InnerException?.Message ?? tie.Message;
        }
      }
      catch (Exception ex)
      {
        return; // ex.Message;
      }

      using (Py.GIL())
      {
        try
        {
          dynamic sys = Py.Import("sys");
          sys.path.append("Scripts");

          dynamic module = Py.Import("analyze");
          // module.readshp(@"C:\shp\tl_2025_us_state.shp");

          // module.readfeature(@"C:\shp\tl_2025_us_state.shp", 2);

          module.describegeometry(@"C:\shp\tl_2025_us_state.shp", 2);
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

      if (PythonEngine.IsInitialized)
      {
        try
        {
          Py.GIL();
          PythonEngine.Shutdown();
        }
        catch (PlatformNotSupportedException)
        {
          // Ignore the exception as the shutdown likely proceeded enough
        }
        catch (PythonException)
        {

        }

        Console.ReadKey();
      }

    }
  }
}