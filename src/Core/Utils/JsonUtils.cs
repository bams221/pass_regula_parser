using System.Text.Json.Nodes;

namespace PassRegulaParser.Core.Utils;

public static class JsonUtils
{
    public static string FindValueByFieldName(
        JsonArray fieldList,
        string fieldName,
        string valuePropertyName = "value",
        string namePropertyName = "fieldName",
        string? languagePropertyName = null,
        string? language = null)
    {
        foreach (var field in fieldList)
        {
            if (field == null) continue;

            var fieldNode = field.AsObject();
            
            if (fieldNode[namePropertyName]?.GetValue<string>() != fieldName)
                continue;

            if (language != null && languagePropertyName != null 
                && fieldNode[languagePropertyName]?.GetValue<string>() != language)
                continue;

            return fieldNode[valuePropertyName]?.GetValue<string>() ?? string.Empty;
        }
        
        return string.Empty;
    }
}