using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Ui;

public class DocumentFormBuilder(DocumentEditWindow window)
{
    private readonly DocumentEditWindow _window = window;
    public Dictionary<string, Control> FieldControls { get; } = [];
    private readonly TableLayoutPanel _mainPanel = CreateMainPanel();

    private static TableLayoutPanel CreateMainPanel()
    {
        return new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(10),
            AutoScroll = true
        };
    }

    public void BuildForm()
    {
        InitializeFields();
        AddAgreementCheckboxAndDaysField();
        AddSaveButton();
        _window.Controls.Add(_mainPanel);
    }

    private void InitializeFields()
    {
        _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        AddReadOnlyField("Тип документа:", nameof(PassportData.DocumentType));
        AddEditableField("ФИО:", nameof(PassportData.FullName), false, FieldValidators.OnlyWordsAndSpaces);
        AddEditableField("Серия:", nameof(PassportData.Serial), false, FieldValidators.OnlyDigits);
        AddEditableField("Номер:", nameof(PassportData.Number), false, FieldValidators.OnlyDigits);
        AddEditableField("Город рождения:", nameof(PassportData.BirthCity), false, FieldValidators.NotEmpty);
        AddEditableField("Дата рождения:", nameof(PassportData.BirthDate), false, FieldValidators.IsDate);
        AddEditableField("Пол:", nameof(PassportData.Gender), false, FieldValidators.OnlyWords);
        AddEditableField("Дата выдачи:", nameof(PassportData.IssueDate), false, FieldValidators.IsDate);
        AddEditableField("Орган выдачи:", nameof(PassportData.Authority), true, FieldValidators.NotEmpty);
        AddEditableField("Код подразделения:", nameof(PassportData.AuthorityCode), false, FieldValidators.NotEmpty);
        AddPhotoField();
        AddEditableField("Описание:", nameof(PassportData.Description), true, FieldValidators.Any);
    }

    private void AddReadOnlyField(string labelText, string propertyName)
    {
        var propertyValue = _window.GetPropertyValue(propertyName)?.ToString() ?? "";
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;
        var label = new Label
        {
            Text = labelText,
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
        _mainPanel.Controls.Add(label, 0, rowIndex);
        _mainPanel.Controls.Add(valueLabel, 1, rowIndex);
        FieldControls.Add(propertyName, valueLabel);
    }

    private void AddEditableField(
        string labelText,
        string propertyName,
        bool isMultiline,
        Func<string, bool> isValid)
    {
        var propertyValue = _window.GetPropertyValue(propertyName)?.ToString() ?? "";
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;
        var label = new Label
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 5, 10, 5)
        };
        Control inputControl = isMultiline
            ? CreateMultilineTextBox(propertyValue)
            : CreateSingleLineTextBox(propertyValue);

        UpdateControlBackground(inputControl, propertyValue, isValid);

        if (inputControl is TextBox textBox)
        {
            textBox.TextChanged += (sender, e) =>
            {
                UpdateControlBackground(textBox, textBox.Text, isValid);
            };
        }

        _mainPanel.Controls.Add(label, 0, rowIndex);
        _mainPanel.Controls.Add(inputControl, 1, rowIndex);
        FieldControls.Add(propertyName, inputControl);
    }

    private static TextBox CreateSingleLineTextBox(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Margin = new Padding(0, 5, 0, 5)
    };

    private static TextBox CreateMultilineTextBox(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Multiline = true,
        ScrollBars = ScrollBars.Vertical,
        Height = 60,
        Margin = new Padding(0, 5, 0, 5)
    };

    private static void UpdateControlBackground(
        Control control,
        string text,
        Func<string, bool>? isValid = null)
    {
        bool isInvalid = isValid != null && !isValid(text);
        control.BackColor = isInvalid
            ? Color.FromArgb(255, 220, 180)
            : SystemColors.Window;
    }

    private void AddPhotoField()
    {
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        _mainPanel.RowCount++;
        var photoLabel = new Label
        {
            Text = "Фото:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 5, 10, 5)
        };
        var photoBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom
        };
        LoadPhotoIntoPictureBox(photoBox);
        _mainPanel.Controls.Add(photoLabel, 0, rowIndex);
        _mainPanel.Controls.Add(photoBox, 1, rowIndex);
        FieldControls.Add(nameof(PassportData.PhotoBase64), photoBox);
    }

    private void LoadPhotoIntoPictureBox(PictureBox photoBox)
    {
        var photoBase64 = _window.GetPropertyValue(nameof(PassportData.PhotoBase64)) as string;
        if (!string.IsNullOrEmpty(photoBase64))
        {
            try
            {
                byte[]? imageBytes = Convert.FromBase64String(photoBase64);
                using var ms = new MemoryStream(imageBytes);
                photoBox.Image = Image.FromStream(ms);
            }
            catch
            {
                SetErrorState(photoBox, "Не удалось загрузить фото");
            }
        }
        else
        {
            SetErrorState(photoBox, "Фото отсутствует");
        }
    }

    private static void SetErrorState(PictureBox photoBox, string message)
    {
        photoBox.Image = null;
        photoBox.BackColor = Color.LightGray;
        photoBox.Paint += (sender, e) =>
        {
            using var font = new Font("Arial", 9);
            var textSize = e.Graphics.MeasureString(message, font);
            var x = (photoBox.Width - textSize.Width) / 2;
            var y = (photoBox.Height - textSize.Height) / 2;
            e.Graphics.DrawString(message, font, Brushes.Black, x, y);
        };
    }

    private void AddAgreementCheckboxAndDaysField()
    {
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;

        var agreementCheckBox = new CheckBox
        {
            Text = "Даю согласие на обработку и хранение персональных данных",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 5, 0, 5),
            AutoSize = true
        };
        _mainPanel.SetColumnSpan(agreementCheckBox, 2);
        _mainPanel.Controls.Add(agreementCheckBox, 0, rowIndex);

        // Поле для ввода количества дней (по умолчанию скрыто)
        var daysRowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;

        var daysLabel = new Label
        {
            Text = "Срок хранения (дней):",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 5, 10, 5)
        };

        var daysTextBox = new TextBox
        {
            Text = _window.GetPropertyValue("DataSaveAgreementDateEnd")?.ToString() ?? "",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 5, 0, 5)
        };

        _mainPanel.Controls.Add(daysLabel, 0, daysRowIndex);
        _mainPanel.Controls.Add(daysTextBox, 1, daysRowIndex);

        // Логика показа/скрытия поля в зависимости от чекбокса
        daysTextBox.Visible = agreementCheckBox.Checked;
        daysLabel.Visible = agreementCheckBox.Checked;

        agreementCheckBox.CheckedChanged += (sender, e) =>
        {
            daysTextBox.Visible = agreementCheckBox.Checked;
            daysLabel.Visible = agreementCheckBox.Checked;
        };

        FieldControls.Add(nameof(PassportData.DataSaveAgreement), agreementCheckBox);
        FieldControls.Add(nameof(PassportData.DataSaveAgreementDateEnd), daysTextBox);
    }

    private void AddSaveButton()
    {
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        _mainPanel.RowCount++;
        var saveButton = new Button
        {
            Text = "Сохранить",
            Dock = DockStyle.Fill,
            Height = 40,
            Margin = new Padding(0, 10, 0, 0)
        };
        saveButton.Click += (sender, e) => _window.SaveAndClose();
        _mainPanel.SetColumnSpan(saveButton, 2);
        _mainPanel.Controls.Add(saveButton, 0, rowIndex);
    }
}
