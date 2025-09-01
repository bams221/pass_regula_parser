using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Nodes;

public class UsernameNode() : INodeElement
{
    public PassportData Process(PassportData passportData)
    {
        PassportData newPassportData = passportData.Clone();
        newPassportData.Username = GetSystemUserName();

        return newPassportData;
    }

    private static string GetSystemUserName()
    {
        return Environment.UserName;
    }
}