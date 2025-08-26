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
        var photoBuilder = new PhotoFieldBuilder(_mainPanel, _window, FieldControls);
        var agreementBuilder = new AgreementFieldBuilder(_mainPanel, _window, FieldControls);
        var buttonBuilder = new SaveButtonBuilder(_mainPanel, _window);

        _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

        new ReadOnlyField(
            _mainPanel, "Тип документа:", nameof(PassportData.DocumentType), _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "ФИО:", nameof(PassportData.FullName),
            FieldValidators.OnlyWordsAndSpaces, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Серия:", nameof(PassportData.Serial),
            FieldValidators.OnlyDigits, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Номер:", nameof(PassportData.Number),
            FieldValidators.OnlyDigits, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Место рождения:", nameof(PassportData.BirthCity),
            FieldValidators.NotEmpty, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Дата рождения:", nameof(PassportData.BirthDate),
            FieldValidators.IsDate, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Пол:", nameof(PassportData.Gender),
            FieldValidators.OnlyWords, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Дата выдачи:", nameof(PassportData.IssueDate),
            FieldValidators.IsDate, _window, FieldControls
            ).AddToPanel();

        new EditableMultilineField(
                _mainPanel, "Орган выдачи:", nameof(PassportData.Authority),
                FieldValidators.NotEmpty, _window, FieldControls
            ).AddToPanel();

        new EditableField(
            _mainPanel, "Код подразделения:", nameof(PassportData.AuthorityCode),
            FieldValidators.NotEmpty, _window, FieldControls
            ).AddToPanel();

        photoBuilder.AddPhotoField();
        
        new EditableMultilineField(
            _mainPanel, "Описание:", nameof(PassportData.Description),
            FieldValidators.Any, _window, FieldControls
            ).AddToPanel();

        agreementBuilder.AddAgreementCheckboxAndDaysField();
        buttonBuilder.AddSaveButton();
        
        _window.Controls.Add(_mainPanel);
    }
}
