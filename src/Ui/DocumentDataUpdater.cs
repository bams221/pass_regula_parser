using PassRegulaParser.Models;

namespace PassRegulaParser.Ui;

public class DocumentDataUpdater(PassportData documentData)
{
    private readonly PassportData _documentData = documentData;

    public void UpdateFromControls(Dictionary<string, Control> fieldControls)
    {
        foreach (var field in fieldControls)
        {
            var control = field.Value;
            var value = control switch
            {
                TextBox textBox => textBox.Text,
                Label label => label.Text,
                CheckBox checkBox => checkBox.Checked.ToString(),
                _ => null
            };

            if (value != null)
            {
                SetPropertyValue(field.Key, value);
            }
        }
    }

    private void SetPropertyValue(string propertyName, object value)
    {
        var property = _documentData.GetType().GetProperty(propertyName);
        property?.SetValue(_documentData, value);
    }
}