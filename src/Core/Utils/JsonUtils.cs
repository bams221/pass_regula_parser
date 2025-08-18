using System.Text.Json.Nodes;

namespace PassRegulaParser.Core.Utils;

public static class JsonUtils
{
    public static string FindFieldValueByFieldName(JsonArray fieldList, string fieldName, string? language = null)
    {
        foreach (var field in fieldList)
        {
            if (field == null) continue;

            var fieldNode = field.AsObject();
            if (fieldNode["fieldName"]?.GetValue<string>() != fieldName)
                continue;

            if (language != null && fieldNode["lcidName"]?.GetValue<string>() != language)
                continue;

            return fieldNode["value"]?.GetValue<string>() ?? string.Empty;
        }
        return string.Empty;
    }
}