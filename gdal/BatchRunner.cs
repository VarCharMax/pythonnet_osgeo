using System.Diagnostics;

class BatchRunner
{
  public static void Run(string batchFilePath)
  {
    // Ensure the file exists (optional, but good practice)
    if (!File.Exists(batchFilePath))
    {
      Console.WriteLine($"Error: Batch file not found at {batchFilePath}");
      return;
    }

    // Configure the process start information
    ProcessStartInfo startInfo = new()
    {
      FileName = "cmd.exe", // The executable used to run batch files
                            // /C executes the command and then terminates the command window.
                            // Use /K instead to execute the command and keep the window open.
      Arguments = $"/C \"{batchFilePath}\"",
      UseShellExecute = false,
      RedirectStandardOutput = false, // Allows capturing the output
      // CreateNoWindow = true, // Hides the command prompt window
      WorkingDirectory = Path.GetDirectoryName(batchFilePath) // Set the working directory
    };

    try
    {
      // Start the process
      using Process? process = Process.Start(startInfo);
      if (process == null)
      {
        Console.WriteLine("Failed to start the process.");
        return;
      }
      // string output = process.StandardOutput.ReadToEnd();

      // Wait for the process to exit
      process.WaitForExit();
      process.Close();
      // Console.WriteLine("Batch file output:");
      // Console.WriteLine(output);
      Console.WriteLine($"Batch file exited with code: {process.ExitCode}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred: {ex.Message}");
    }
  }
}
