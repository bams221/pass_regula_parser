using PassRegulaParser.Models;
using PassRegulaParser.Ui.Utils;

namespace PassRegulaParser.Ui;

public class DocumentEditWindow : Form
{
    private readonly PassportData _documentData;
    private readonly DocumentFormBuilder _formBuilder;
    private readonly DocumentDataUpdater _dataUpdater;

    public DocumentEditWindow(PassportData passportData)
    {
        _documentData = passportData;
        _formBuilder = new DocumentFormBuilder(this);
        _dataUpdater = new DocumentDataUpdater(_documentData);

        InitializeWindowProperties();
        _formBuilder.BuildForm();
    }

    private void InitializeWindowProperties()
    {
        Text = "Данные отсканированного документа";
        Size = new Size(500, 800);
        MinimumSize = new Size(400, 600);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        TopMost = true;
    }

    public void SaveAndClose()
    {
        _dataUpdater.UpdateFromControls(_formBuilder.FieldControls);
        DocumentDataSaver.SaveToJson(_documentData);
        DialogResult = DialogResult.OK;
        Close();
    }

    public PassportData GetDocumentData() => _documentData;

    public object? GetPropertyValue(string propertyName)
    {
        var property = typeof(PassportData).GetProperty(propertyName);
        return property?.GetValue(_documentData);
    }

    public T? GetPropertyValue<T>(string propertyName)
    {
        var value = GetPropertyValue(propertyName);
        return value is T typedValue ? typedValue : default;
    }
}