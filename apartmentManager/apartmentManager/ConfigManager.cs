using System.IO;

public static class ConfigManager
{
    private const string ConfigFilePath = "config.txt";

    public static string GetAdminPassword()
    {
        if (File.Exists(ConfigFilePath))
        {
            return File.ReadAllText(ConfigFilePath);
        }
        return null; // Handle case where file doesn't exist
    }

    public static void SetAdminPassword(string password)
    {
        File.WriteAllText(ConfigFilePath, password);
    }
}
