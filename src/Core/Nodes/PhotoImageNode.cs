using PassRegulaParser.Core.Dto;
using PassRegulaParser.Core.Interfaces;

namespace PassRegulaParser.Core.Nodes;

class PhotoImageNode(string photoFilepath) : INodeElement
{
    readonly string _photoFilepath = photoFilepath;

    public PassportData Process(PassportData passportData)
    {
        string photoBase64 = GetPhotoBase64FromFile(_photoFilepath);
        passportData.PhotoBase64 = photoBase64;
        return passportData;
    }

    private string GetPhotoBase64FromFile(string photoFilepath)
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
