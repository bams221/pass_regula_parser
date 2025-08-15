using PassRegulaParser.Model;
using PassRegulaParser.Services;

namespace PassRegulaParser.Ui;

public class DocumentEditWindow : Form
{
    private PassportData _documentData;

    public DocumentEditWindow(PassportData passportData)
    {
        _documentData = passportData;
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
            RowCount = 9,
            Padding = new Padding(10),
            AutoSize = true
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

        AddLabel(panel, "Тип документа:", _documentData.DocumentType ?? "", 0);
        AddField(panel, "ФИО:", _documentData.FullName ?? "", 1);
        AddField(panel, "Серия/номер:", _documentData.SerialNumber ?? "", 2);
        AddField(panel, "Город рождения:", _documentData.BirthCity ?? "", 3);
        AddField(panel, "Дата рождения:", _documentData.BirthDate ?? "", 4);
        AddField(panel, "Пол:", _documentData.Gender ?? "", 5);
        AddField(panel, "Описание:", _documentData.Description ?? "", 6);

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

        if (!string.IsNullOrEmpty(_documentData.PhotoBase64))
        {
            try
            {
                var imageBytes = Convert.FromBase64String(_documentData.PhotoBase64);
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

        panel.Controls.Add(photoLabel, 0, 7);
        panel.Controls.Add(photoBox, 1, 7);

        var saveButton = new Button
        {
            Text = "Сохранить",
            Dock = DockStyle.Bottom,
            Height = 40
        };
        saveButton.Click += (sender, e) => SaveButton_Click();

        panel.SetColumnSpan(saveButton, 2);
        panel.Controls.Add(saveButton, 0, 8);

        Controls.Add(panel);
    }

    private void SaveButton_Click()
    {
        UpdateDocumentDataFromTextBlocks();
        
        DocumentDataSaver.SaveToJson(_documentData);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void UpdateDocumentDataFromTextBlocks()
    {
        foreach (Control control in Controls[0].Controls)
        {
            if (control is TextBox textBox)
            {
                var row = ((TableLayoutPanel)Controls[0]).GetRow(textBox);
                switch (row)
                {
                    case 1: _documentData.FullName = textBox.Text; break;
                    case 2: _documentData.SerialNumber = textBox.Text; break;
                    case 3: _documentData.BirthCity = textBox.Text; break;
                    case 4: _documentData.BirthDate = textBox.Text; break;
                    case 5: _documentData.Gender = textBox.Text; break;
                    case 6: _documentData.Description = textBox.Text; break;
                }
            }
        }
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