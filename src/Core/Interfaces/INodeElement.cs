using PassRegulaParser.Core.Dto;

namespace PassRegulaParser.Core.Interfaces;

public interface INodeElement
{
    public PassportData Process(PassportData passportData);
}