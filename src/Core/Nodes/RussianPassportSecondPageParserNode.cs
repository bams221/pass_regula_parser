using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using PassRegulaParser.Model;

namespace PassRegulaParser.Core.Nodes;

class RussianPassportSecondPageParserNode: INodeElement
{
    readonly string _secondPageFolderPath;
    readonly bool _isFolderExists;

    public RussianPassportSecondPageParserNode(string secondPageFolderPath)
    {
        _secondPageFolderPath = secondPageFolderPath;
        _isFolderExists = FolderChecker.IsFolderExists(_secondPageFolderPath);
    }

    public PassportData Process(PassportData passportData)
    {
        if (!_isFolderExists)
        {
            Console.WriteLine($"Folder {_secondPageFolderPath} does not exists. Skipping");
            return passportData;
        }

        

        return passportData;
    }

}