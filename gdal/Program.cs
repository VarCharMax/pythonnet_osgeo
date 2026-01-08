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
      // string pathToVirtualEnv = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", ".venv");
      // string message;

      string osgeoRoot = @"C:\Users\rpark\AppData\Local\Programs\OSGeo4W";

      string runtime = "python312.dll";
      string pDllPath = @$"{osgeoRoot}\apps\Python312\{runtime}";

      Environment.SetEnvironmentVariable("PYTHONPATH", "", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PYTHONUTF8", "1", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pDllPath, EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PYTHONHOME", @$"{osgeoRoot}\apps\Python312", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("OSGEO4W_ROOT", osgeoRoot, EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PATH", @$"{osgeoRoot}\apps\Python312\Scripts;C:\Users\rpark\AppData\Local\Programs\OSGeo4W\bin;C:\WINDOWS\system32;C:\WINDOWS;C:\WINDOWS\system32\WBem", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("GDAL_DATA", @$"{osgeoRoot}\apps\gdal\share\gdal", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("GDAL_DRIVER_PATH", @$"{osgeoRoot}\apps\gdal\lib\gdalplugins", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("PROJ_DATA", @$"{osgeoRoot}\share\proj", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("OPENSSL_ENGINES", @$"{osgeoRoot}\lib\engines-3", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("SSL_CERT_FILE", @$"{osgeoRoot}\bin\curl-ca-bundle.crt", EnvironmentVariableTarget.Process);
      Environment.SetEnvironmentVariable("SSL_CERT_DIR", @$"{osgeoRoot}\apps\openssl\certs", EnvironmentVariableTarget.Process);

      Thread myThread = new(new ThreadStart(() => BatchRunner.Run(Path.Combine(@$"{osgeoRoot}\bin", "o4w_env.bat"))))
      {
        IsBackground = true
      };
      // myThread.Start();

      // myThread.Join();

      

      // Runtime.PythonDLL = @$"{osgeoRoot}\apps\Python312\{runtime}";

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
          sys.path.append(osgeoRoot);
          sys.path.append(@$"{osgeoRoot}\apps\Python312\Scripts");
          sys.path.append(@$"{osgeoRoot}\bin");
          sys.path.append(@$"{osgeoRoot}\apps\gdal\share\gdal");
          sys.path.append(@$"{osgeoRoot}\apps\gdal\lib\gdalplugins");

          dynamic module = Py.Import("analyze");
          module.readshp(@"C:\dev\dotnet\gdal\gdal\bin\Debug\net9.0\Scripts\tl_2025_us_state.shp");
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
          // AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true); // No longer works in .NET 8+
          Py.GIL();
          PythonEngine.Shutdown();
          // AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", false); // Causes a corrupted memory exxception.
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