using PassRegulaParser.Core.Dto;

namespace PassRegulaParser.Ui;

public class DocumentEditForm : Form
{
    private readonly PassportData _passportData;

    public DocumentEditForm(PassportData passportData)
    {
        _passportData = passportData;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Данные отсканированного документа";
        Size = new Size(400, 600);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(10),
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

        AddLabel(panel, "Тип документа:", _passportData.DocumentType ?? "-", 0);
        AddField(panel, "ФИО:", _passportData.FullName ?? "-", 1);
        AddField(panel, "Серия/номер:", _passportData.SerialNumber ?? "-", 2);
        AddField(panel, "Город рождения:", _passportData.BirthCity ?? "-", 3);
        AddField(panel, "Дата рождения:", _passportData.BirthDate ?? "-", 4);
        AddField(panel, "Пол:", _passportData.Gender ?? "-", 5);

        var photoLabel = new Label
        {
            Text = "Фото:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        var photoBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            Height = 150
        };

        if (!string.IsNullOrEmpty(_passportData.PhotoBase64))
        {
            try
            {
                var imageBytes = Convert.FromBase64String(_passportData.PhotoBase64);
                using (var ms = new MemoryStream(imageBytes))
                {
                    photoBox.Image = Image.FromStream(ms);
                }
            }
            catch
            {
                photoBox.Image = null;
                photoBox.BackColor = Color.LightGray;
                photoBox.Text = "Не удалось загрузить фото";
            }
        }
        else
        {
            photoBox.BackColor = Color.LightGray;
            photoBox.Text = "Фото отсутствует";
        }

        panel.Controls.Add(photoLabel, 0, 6);
        panel.Controls.Add(photoBox, 1, 6);

        var saveButton = new Button
        {
            Text = "Сохранить",
            Dock = DockStyle.Bottom,
            Height = 40
        };
        saveButton.Click += (sender, e) =>
        {
            DialogResult = DialogResult.OK;
            Close();
        };

        panel.SetColumnSpan(saveButton, 2);
        panel.Controls.Add(saveButton, 0, 7);

        Controls.Add(panel);
    }

    private static void AddField(TableLayoutPanel panel, string labelText, string value, int row)
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

    private static void AddLabel(TableLayoutPanel panel, string labelText, string value, int row)
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