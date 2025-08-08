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

    /// <summary>
    /// Ensure the
    /// </summary>
    /// <exception cref="Exception"></exception>
    [Test]
    public void TestGridPointsWithinRadius()
    {
        var points = MathUtilities.GetGridPointsWithinRadius(latitude: -27.2874563, longitude: 151.2620588, radius: 50,
                                                             resolution: 0.25, offset: 0.125)
                                  .ToArray();
        Assert.That(points, Has.Length.EqualTo(11));

        (double, double)[] expectedPoints = {
            (-27.125, 151.125),
            (-27.125, 151.375),
            (-27.125, 151.625),
            (-27.375, 150.875),
            (-27.375, 151.125),
            (-27.375, 151.375),
            (-27.375, 151.625),
            (-27.375, 151.875),
            (-27.625, 151.125),
            (-27.625, 151.375),
            (-27.625, 151.625) };
        Assert.That(points, Is.EqualTo(expectedPoints));
    }
}