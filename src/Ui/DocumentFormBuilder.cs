using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;
using PassRegulaParser.Ui.FieldBuilders;

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
        _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

        InitializeFields();
        var photoBuilder = new PhotoFieldBuilder(_mainPanel, _window, FieldControls);
        photoBuilder.AddPhotoField();

        var agreementBuilder = new AgreementFieldBuilder(_mainPanel, _window, FieldControls);
        agreementBuilder.AddAgreementCheckboxAndDaysField();

        var buttonBuilder = new SaveButtonBuilder(_mainPanel, _window);
        buttonBuilder.AddSaveButton();
        _window.Controls.Add(_mainPanel);
    }

    private void InitializeFields()
    {
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
}
