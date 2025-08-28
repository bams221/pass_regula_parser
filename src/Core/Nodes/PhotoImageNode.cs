using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

class PhotoImageNode(string photoFilepath) : INodeElement
{
    readonly string _photoFilepath = photoFilepath;

    public PassportData Process(PassportData passportData)
    {
        PassportData newPassportData = passportData.Clone();
        string photoBase64 = PhotoUtils.GetPhotoBase64FromFile(_photoFilepath);
        newPassportData.PhotoBase64 = photoBase64;
        return newPassportData;
    }

}
