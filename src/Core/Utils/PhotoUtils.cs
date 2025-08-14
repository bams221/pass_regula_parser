namespace PassRegulaParser.Core.Utils;

public static class PhotoUtils
{
    public static string GetPhotoBase64FromFile(string photoFilepath)
    {
        if (!File.Exists(photoFilepath))
        {
            return string.Empty;
        }

        string extension = Path.GetExtension(photoFilepath).ToLower();
        if (extension != ".jpg" && extension != ".jpeg")
        {
            return string.Empty;
        }

        byte[] imageBytes = File.ReadAllBytes(photoFilepath);
        return Convert.ToBase64String(imageBytes);
    }
}