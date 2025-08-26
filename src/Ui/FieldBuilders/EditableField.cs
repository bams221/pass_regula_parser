namespace PassRegulaParser.Ui.FieldBuilders;

public class EditableField
{
    private readonly TableLayoutPanel _panel;
    private readonly string _labelText;
    private readonly string _propertyName;
    private readonly Func<string, bool> _validator;
    private readonly DocumentEditWindow _window;
    private readonly Dictionary<string, Control> _fieldControls;

    public EditableField(
        TableLayoutPanel panel,
        string labelText,
        string propertyName,
        Func<string, bool> validator,
        DocumentEditWindow window,
        Dictionary<string, Control> fieldControls)
    {
        _panel = panel;
        _labelText = labelText;
        _propertyName = propertyName;
        _validator = validator;
        _window = window;
        _fieldControls = fieldControls;
    }

    public void AddToPanel()
    {
        var propertyValue = _window.GetPropertyValue(_propertyName)?.ToString() ?? "";
        var rowIndex = _panel.RowCount;
        _panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _panel.RowCount++;

        var label = new Label
        {
            Text = _labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 5, 10, 5)
        };

        var textBox = new TextBox
        {
            Text = propertyValue,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 5, 0, 5)
        };

        UpdateBackground(textBox, propertyValue);

        textBox.TextChanged += (sender, e) =>
        {
            UpdateBackground(textBox, textBox.Text);
        };

        _panel.Controls.Add(label, 0, rowIndex);
        _panel.Controls.Add(textBox, 1, rowIndex);
        _fieldControls.Add(_propertyName, textBox);
    }

    private void UpdateBackground(TextBox textBox, string text)
    {
        textBox.BackColor = _validator(text)
            ? SystemColors.Window
            : Color.FromArgb(255, 220, 180);
    }
}
