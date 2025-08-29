namespace PassRegulaParser.Ui.FieldBuilders;

public class SaveButtonBuilder(TableLayoutPanel mainPanel, DocumentEditWindow window)
{
    private readonly TableLayoutPanel _mainPanel = mainPanel;
    private readonly DocumentEditWindow _window = window;

    public void AddSaveButton()
    {
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        _mainPanel.RowCount++;

        var saveButton = new Button
        {
            Text = "Отправить",
            Dock = DockStyle.Fill,
            Height = 40,
            Margin = new Padding(0, 10, 0, 0)
        };

        saveButton.Click += async (sender, e) =>
        {
            saveButton.Enabled = false;
            saveButton.Text = "Отправка...";
            await _window.SendDataAndCloseIfSuccess();
            saveButton.Text = "Отправить";
            saveButton.Enabled = true; 
        };

        _mainPanel.SetColumnSpan(saveButton, 2);
        _mainPanel.Controls.Add(saveButton, 0, rowIndex);
    }
}
