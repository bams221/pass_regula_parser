namespace PassRegulaParser.Ui;

public static class ControlHelper
{
    public static void AddField(TableLayoutPanel panel, string labelText, string value, int row)
    {
        var label = new Label
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var textBox = new TextBox
        {
            Text = value,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 5, 0, 5)
        };

        panel.Controls.Add(label, 0, row);
        panel.Controls.Add(textBox, 1, row);
    }

    public static void AddLabel(TableLayoutPanel panel, string labelText, string value, int row)
    {
        var labelTitle = new Label
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var labelValue = new Label
        {
            Text = value,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        panel.Controls.Add(labelTitle, 0, row);
        panel.Controls.Add(labelValue, 1, row);
    }
}
