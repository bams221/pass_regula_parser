using PassRegulaParser.Core.Dto;

namespace PassRegulaParser.Ui;

public class PassportEditForm : Form
{
    private readonly PassportData _passportData;

    public PassportEditForm(PassportData passportData)
    {
        _passportData = passportData;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Редактирование данных паспорта";
        Size = new Size(400, 500);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(10),
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

        AddField(panel, "Тип документа:", _passportData.DocumentType ?? "", 0);
        AddField(panel, "ФИО:", _passportData.FullName ?? "", 1);
        AddField(panel, "Серия/номер:", _passportData.SerialNumber ?? "", 2);
        AddField(panel, "Город рождения:", _passportData.BirthCity ?? "", 3);
        AddField(panel, "Дата рождения:", _passportData.BirthDate ?? "", 4);
        AddField(panel, "Пол:", _passportData.Gender ?? "", 5);

        var saveButton = new Button
        {
            Text = "Сохранить",
            Dock = DockStyle.Bottom,
            Height = 40
        };
        saveButton.Click += (sender, e) =>
        {
            MessageBox.Show("Saving data");
            DialogResult = DialogResult.OK;
            Close();
        };

        panel.SetColumnSpan(saveButton, 2);
        panel.Controls.Add(saveButton, 0, 6);

        Controls.Add(panel);
    }

    private void AddField(TableLayoutPanel panel, string labelText, string value, int row)
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
}