using PassRegulaParser.Models;
using System.Windows.Forms;
using PassRegulaParser.Ui;

namespace PassRegulaParser.Tests.Ui;

public class DocumentDataUpdaterTests
{
    [Fact]
    public void UpdateFromControls_ShouldUpdateDocumentProperties()
    {
        var passportData = new PassportData();
        var updater = new DocumentDataUpdater(passportData);
        var controls = new Dictionary<string, Control>
        {
            { "FullName", new TextBox { Text = "Иванов Иван Иванович" } },
            { "SerialNumber", new TextBox { Text = "1234567890" } }
        };

        updater.UpdateFromControls(controls);

        Assert.Equal("Иванов Иван Иванович", passportData.FullName);
        Assert.Equal("1234567890", passportData.SerialNumber);
    }
}