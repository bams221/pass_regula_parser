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

    public static string FindBufTextByFieldName(JsonArray fieldList, string fieldName)
    {
        foreach (var field in fieldList)
        {
            if (field == null) continue;

            var fieldNode = field.AsObject();
            if (fieldNode["FieldName"]?.GetValue<string>() != fieldName)
                continue;

            return fieldNode["Buf_Text"]?.GetValue<string>() ?? string.Empty;
        }
        return string.Empty;
    }
}