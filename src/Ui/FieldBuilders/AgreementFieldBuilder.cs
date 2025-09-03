using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;
using PassRegulaParser.Ui.Utils;

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
        int rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;

        CheckBox agreementCheckBox = CreateAgreementCheckBox("Даю согласие на обработку и хранение персональных данных");
        _mainPanel.SetColumnSpan(agreementCheckBox, 2);
        _mainPanel.Controls.Add(agreementCheckBox, 0, rowIndex);

        int daysRowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainPanel.RowCount++;

        FlowLayoutPanel flowPanel = CreateFlowPanel();

        Label daysLabel = CreateDaysLabel("Срок хранения до:");

        TextBox  daysTextBox = CreateDaysTextBox();

        ComboBox periodComboBox = CreatePeriodComboBox(daysTextBox);

        flowPanel.Controls.Add(daysTextBox);
        flowPanel.Controls.Add(periodComboBox);

        _mainPanel.Controls.Add(daysLabel, 0, daysRowIndex);
        _mainPanel.Controls.Add(flowPanel, 1, daysRowIndex);

        _fieldControls.Add(nameof(PassportData.DataSaveAgreement), agreementCheckBox);
        _fieldControls.Add(nameof(PassportData.DataSaveAgreementDateEnd), daysTextBox);
    }

    private static CheckBox CreateAgreementCheckBox(string text)
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

    private static FlowLayoutPanel CreateFlowPanel()
    {
        return new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
        };
    }

    private static Label CreateDaysLabel(string text)
    {
        return new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, DefaultPadding, DefaultPadding, DefaultPadding)
        };
    }

    private TextBox CreateDaysTextBox()
    {
        TextBox textBox = new()
        {
            Text = _window.GetPropertyValue(nameof(PassportData.DataSaveAgreementDateEnd))?.ToString() ?? "",
            Width = TextBoxWidth,
            Margin = new Padding(0, DefaultPadding, 0, DefaultPadding),
            MaxLength = DateMaxLength
        };
        textBox.KeyPress += (sender, e) =>
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        };
        textBox.TextChanged += (sender, e) =>
        {
            TextBox tb = (TextBox)sender!;
            string formattedText = DateFormatter.FormatDateText(tb.Text);
            if (tb.Text != formattedText)
            {
                tb.Text = formattedText;
                tb.SelectionStart = formattedText.Length;
            }
        };
        return textBox;
    }

    private static ComboBox CreatePeriodComboBox(TextBox daysTextBox)
    {
        ComboBox comboBox = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, DefaultPadding, 0, DefaultPadding),
            DropDownStyle = ComboBoxStyle.DropDownList,
            SelectedItem = PeriodOptions[0],
        };
        comboBox.Items.AddRange(PeriodOptions);
        comboBox.SelectedIndex = 0;
        comboBox.SelectedIndexChanged += (sender, e) =>
        {
            string selectedPeriod = comboBox.SelectedItem.ToString()!;
            DateTime endDate = DateUtils.CalculateEndDate(selectedPeriod);
            daysTextBox.Text = endDate.ToString("dd.MM.yyyy");
        };
        return comboBox;
    }
}
