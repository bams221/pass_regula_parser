using System.Text.Json.Nodes;

namespace PassRegulaParser.Core.Utils;

public class FieldList(
    JsonArray fieldArray,
    string valuePropertyName,
    string namePropertyName,
    string? languagePropertyName = null,
    string? language = null
        )
{
    private readonly JsonArray _fieldArray = fieldArray ?? throw new ArgumentNullException(nameof(fieldArray));
    private readonly string _valuePropertyName = valuePropertyName;
    private readonly string _namePropertyName = namePropertyName;
    private readonly string? _languagePropertyName = languagePropertyName;
    private readonly string? _language = language;

    public string GetValue(string fieldName)
    {
        foreach (var field in _fieldArray)
        {
            if (field == null) continue;

            var fieldNode = field.AsObject();

            if (fieldNode[_namePropertyName]?.GetValue<string>() != fieldName)
                continue;

            return fieldNode[_valuePropertyName]?.GetValue<string>() ?? string.Empty;
        }

        return string.Empty;
    }

    public string GetValueRussian(string fieldName)
    {
        if (_languagePropertyName == null || _language == null)
        {
            throw new InvalidOperationException("Language properties were not specified in FieldList constructor.");
        }

        foreach (var field in _fieldArray)
        {
            if (field == null) continue;

            var fieldNode = field.AsObject();

            if (fieldNode[_namePropertyName]?.GetValue<string>() != fieldName)
                continue;

            if (fieldNode[_languagePropertyName]?.GetValue<string>() != _language)
                continue;

            return fieldNode[_valuePropertyName]?.GetValue<string>() ?? string.Empty;
        }

        return string.Empty;
    }
}