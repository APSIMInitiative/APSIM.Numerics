using System.Diagnostics;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

namespace APSIM.Numerics.Tests;

public class Tests
{
    /// <summary>
    /// Ensure the version number in the .csproj file has been changed. Otherwise the push to NuGet will fail
    /// in GitHub Actions.
    /// </summary>
    /// <exception cref="Exception"></exception>
    [Test]
    public void EnsureVersionHasBeenIncremented()
    {
        string nugetPackageName = "APSIM.Numerics";

        string stdOut = string.Empty;
        var process = Process.Start("dotnet", $"package search {nugetPackageName} --format json");
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += (sender, args) => stdOut += args.Data;
        process.Start();
        process.BeginOutputReadLine();
        process.WaitForExit();
        if (process.ExitCode != 0)
            throw new Exception($"Cannot get version from NuGet. Error: {process.StandardError}");

        dynamic? array = JsonConvert.DeserializeObject(stdOut);
        if (array == null)
            throw new Exception("No response from NuGet");

        var latestVersion = array.searchResult[0].packages[0].latestVersion.ToString();
        var fileVersion = FileVersionInfo.GetVersionInfo($"{nugetPackageName}.dll").FileVersion;
        Assert.That(fileVersion, Does.Not.StartWith(latestVersion));
    }
}