namespace PassRegulaParser.Ui.FieldBuilders;

public class ReadOnlyField
{
    private readonly TableLayoutPanel _panel;
    private readonly string _labelText;
    private readonly string _propertyName;
    private readonly DocumentEditWindow _window;
    private readonly Dictionary<string, Control> _fieldControls;

    public ReadOnlyField(
        TableLayoutPanel panel,
        string labelText,
        string propertyName,
        DocumentEditWindow window,
        Dictionary<string, Control> fieldControls)
    {
        _panel = panel;
        _labelText = labelText;
        _propertyName = propertyName;
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

        var valueLabel = new Label
        {
            Text = propertyValue,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 5, 0, 5)
        };

        _panel.Controls.Add(label, 0, rowIndex);
        _panel.Controls.Add(valueLabel, 1, rowIndex);
        _fieldControls.Add(_propertyName, valueLabel);
    }
}
