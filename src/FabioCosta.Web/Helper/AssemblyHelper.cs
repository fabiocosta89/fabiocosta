namespace FabioCosta.Web.Helper;

using System;
using System.Reflection;

public static class AssemblyHelper
{
    /// <summary>
    /// Retrives the assembly version 
    /// </summary>
    /// <returns>format v1.0</returns>
    public static string GetAppVersion()
    {
        Version version = Assembly.GetEntryAssembly().GetName().Version;
        return $"v{version.Major}.{version.Minor}";
    }

    /// <summary>
    /// Retrives the assembly version with build
    /// </summary>
    /// <returns>format v1.0.0</returns>
    public static string GetAppVersionWithBuild()
    {
        Version version = Assembly.GetEntryAssembly().GetName().Version;
        return $"v{version.Major}.{version.Minor}.{version.Build}";
    }
}
