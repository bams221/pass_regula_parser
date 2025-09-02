using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Ui.FieldBuilders;

public class AgreementFieldBuilder
{
    private readonly TableLayoutPanel _mainPanel;
    private readonly DocumentEditWindow _window;
    private readonly Dictionary<string, Control> _fieldControls;


    private const int DateMaxLength = 10; // ДД.ММ.ГГГГ
    private const int TextBoxWidth = 200;
    private const int DefaultPadding = 5;

    private static readonly string[] PeriodOptions = ["1 день", "1 месяц", "1 год", "10 лет"];


    public AgreementFieldBuilder(
        TableLayoutPanel mainPanel,
        DocumentEditWindow window,
        Dictionary<string, Control> fieldControls)
    {
        _mainPanel = mainPanel;
        _window = window;
        _fieldControls = fieldControls;
    }

    public void AddAgreementCheckboxAndDaysField()
    {
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;

        var agreementCheckBox = CreateAgreementCheckBox("Даю согласие на обработку и хранение персональных данных");
        _mainPanel.SetColumnSpan(agreementCheckBox, 2);
        _mainPanel.Controls.Add(agreementCheckBox, 0, rowIndex);

        var daysRowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;


        var flowPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
        };

        var daysLabel = new Label
        {
            Text = "Срок хранения до:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, DefaultPadding, 10, DefaultPadding)
        };

        var daysTextBox = new TextBox
        {
            Text = _window.GetPropertyValue(nameof(PassportData.DataSaveAgreementDateEnd))?.ToString() ?? "",
            Width = 200,
            Margin = new Padding(0, DefaultPadding, 0, DefaultPadding),
            MaxLength = 10 // ДД.ММ.ГГГГ = 10
        };

        daysTextBox.KeyPress += (sender, e) =>
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        };

        daysTextBox.TextChanged += (sender, e) =>
        {
            TextBox textBox = (TextBox)sender!;
            string formattedText = DateFormatter.FormatDateText(textBox.Text);
            if (textBox.Text != formattedText)
            {
                textBox.Text = formattedText;
                textBox.SelectionStart = formattedText.Length;
            }
        };

        ComboBox periodComboBox = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, DefaultPadding, 0, DefaultPadding),
            DropDownStyle = ComboBoxStyle.DropDownList,
            SelectedItem = "1 день",
        };

        periodComboBox.Items.AddRange(PeriodOptions);
        periodComboBox.SelectedIndex = 0;
        periodComboBox.SelectedIndexChanged += (sender, e) =>
        {
            string selectedPeriod = periodComboBox.SelectedItem.ToString()!;
            DateTime endDate = CalculateEndDate(selectedPeriod);
            daysTextBox.Text = endDate.ToString("dd.MM.yyyy");
        };

        flowPanel.Controls.Add(daysTextBox);
        flowPanel.Controls.Add(periodComboBox);

        _mainPanel.Controls.Add(daysLabel, 0, daysRowIndex);
        _mainPanel.Controls.Add(flowPanel, 1, daysRowIndex);

        _fieldControls.Add(nameof(PassportData.DataSaveAgreement), agreementCheckBox);
        _fieldControls.Add(nameof(PassportData.DataSaveAgreementDateEnd), daysTextBox);
    }

    private CheckBox CreateAgreementCheckBox(string text)
    {
        return new CheckBox
        {
            Text = text,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, DefaultPadding, 0, DefaultPadding),
            AutoSize = true,
            Checked = true
        };
    }

    private static DateTime CalculateEndDate(string period)
    {
        DateTime today = DateTime.Today;
        return period switch
        {
            "1 день" => today,
            "1 месяц" => today.AddMonths(1),
            "1 год" => today.AddYears(1),
            "10 лет" => today.AddYears(10),
            _ => today,
        };
    }
}
