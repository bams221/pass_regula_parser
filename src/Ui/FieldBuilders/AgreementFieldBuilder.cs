using PassRegulaParser.Core.Utils;
using PassRegulaParser.Models;

namespace PassRegulaParser.Ui.FieldBuilders;

public class AgreementFieldBuilder
{
    private readonly TableLayoutPanel _mainPanel;
    private readonly DocumentEditWindow _window;
    private readonly Dictionary<string, Control> _fieldControls;

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

        var agreementCheckBox = new CheckBox
        {
            Text = "Даю согласие на обработку и хранение персональных данных",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 5, 0, 5),
            AutoSize = true,
            Checked = true
        };
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
            Margin = new Padding(0, 5, 10, 5)
        };

        var daysTextBox = new TextBox
        {
            Text = _window.GetPropertyValue(nameof(PassportData.DataSaveAgreementDateEnd))?.ToString() ?? "",
            Width = 200,
            Margin = new Padding(0, 5, 0, 5),
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
            Margin = new Padding(0, 5, 0, 5),
            DropDownStyle = ComboBoxStyle.DropDownList,
            SelectedItem = "1 день",
        };

        periodComboBox.Items.AddRange(["1 день", "1 месяц", "1 год", "10 лет"]);
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

    private static DateTime CalculateEndDate(string period)
    {
        DateTime today = DateTime.Today;
        switch (period)
        {
            case "1 день":
                return today;
            case "1 месяц":
                return today.AddMonths(1);
            case "1 год":
                return today.AddYears(1);
            case "10 лет":
                return today.AddYears(10);
            default:
                return today;
        }
    }
}
