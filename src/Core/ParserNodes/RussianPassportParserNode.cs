using PassRegulaParser.Core.Dto;
using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Core.Utils;
using System.Text.Json;

namespace PassRegulaParser.Core.ParserNodes;

class RussianPassportParserNode(string doctypeDataJsonFilepath) : INodeElement
{
    private readonly JsonFileParser _jsonParser = new(doctypeDataJsonFilepath);

    public PassportData Process(PassportData passportData)
    {
        try
        {
            string serialNumber = GetFieldValue("Text.fieldList", 
                f => f.GetProperty("fieldName").GetString() == "Document Number");

            string birthDate = GetFieldValue("Text.fieldList", 
                f => f.GetProperty("fieldName").GetString() == "Date of Birth");

            string fullName = GetFieldValue("Text.fieldList", 
                f => f.GetProperty("fieldName").GetString() == "Surname And Given Names" &&
                     f.TryGetProperty("lcidName", out var lcidName) && 
                     lcidName.GetString() == "Russian");

            string gender = GetFieldValue("Text.fieldList", 
                f => f.GetProperty("fieldName").GetString() == "Sex" &&
                     f.TryGetProperty("lcidName", out var lcidName) && 
                     lcidName.GetString() == "Russian");

            string birthCity = GetFieldValue("Text.fieldList", 
                f => f.GetProperty("fieldName").GetString() == "Place of Birth");

            passportData.SerialNumber = serialNumber;
            passportData.BirthDate = birthDate;
            passportData.FullName = fullName;
            passportData.BirthCity = birthCity;
            passportData.Gender = gender;



            return passportData;
        }
        catch (Exception ex)
        {
            throw new ParsingException("Error processing Russian passport data", ex);
        }
    }

    private string GetFieldValue(string arrayPath, Func<JsonElement, bool> predicate)
    {
        try
        {
            var currentElement = _jsonParser.JsonDocument.RootElement;

            foreach (var property in arrayPath.Split('.'))
            {
                currentElement = currentElement.GetProperty(property);
            }

            foreach (var field in currentElement.EnumerateArray())
            {
                if (predicate(field))
                {
                    if (field.TryGetProperty("value", out var valueProp))
                    {
                        return valueProp.GetString() ?? string.Empty;
                    }
                }
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            throw new ParsingException("Error extracting field value from JSON", ex);
        }
    }
}