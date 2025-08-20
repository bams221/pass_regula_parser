using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Interfaces;

public interface INodeElement
{
    public PassportData Process(PassportData passportData);
}