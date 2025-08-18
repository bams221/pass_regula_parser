namespace PassRegulaParser.Core.Utils;

public static class FolderChecker
{
    public static bool IsFolderExists(string folderPath, int checkDelayMs = 2000)
    {
        int attempts = 3;

        for (int i = 0; i < attempts; i++)
        {
            if (Directory.Exists(folderPath))
            {
                return true;
            }

            if (i < attempts - 1)
            {
                Thread.Sleep(checkDelayMs);
            }
        }

        return false;
    }
}