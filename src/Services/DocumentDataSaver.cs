using System.Text.Json;
using PassRegulaParser.Model;

namespace PassRegulaParser.Services;

public static class DocumentDataSaver
{
    static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

    public static void SaveToJson(PassportData data)
    {
        string json = JsonSerializer.Serialize(data, _options);

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json",
            Title = "Сохранить данные документа",
            FileName = $"{data.FullName?.Replace(" ", "_")}_passport.json"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveFileDialog.FileName, json, System.Text.Encoding.UTF8);
        }
    }
}