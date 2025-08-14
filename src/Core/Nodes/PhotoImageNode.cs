using PassRegulaParser.Core.Dto;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Core.Nodes;

class PhotoImageNode(string photoFilepath) : INodeElement
{
    readonly string _photoFilepath = photoFilepath;

    public PassportData Process(PassportData passportData)
    {
        string photoBase64 = PhotoUtils.GetPhotoBase64FromFile(_photoFilepath);
        passportData.PhotoBase64 = photoBase64;
        return passportData;
    }

}
