using System.Reflection;
using PassRegulaParser.Models;

namespace PassRegulaParser.Ui;

public static class DocumentEditWindowExtensions
{
    public static object? GetPropertyValue(this DocumentEditWindow window, string propertyName)
    {
        var property = typeof(PassportData).GetProperty(propertyName);
        return property?.GetValue(window.GetDocumentData());
    }

    public static PassportData? GetDocumentData(this DocumentEditWindow window)
    {
        var field = typeof(DocumentEditWindow).GetField("_documentData", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        return (PassportData?)field?.GetValue(window);
    }
}