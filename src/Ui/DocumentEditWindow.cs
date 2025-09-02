using PassRegulaParser.Models;
using PassRegulaParser.Api;

namespace PassRegulaParser.Ui;

public class DocumentEditWindow : Form
{
    private readonly PassportData _documentData;
    private readonly DocumentFormBuilder _formBuilder;
    private readonly DocumentDataUpdater _dataUpdater;
    private readonly ApiClient _apiService;

    public DocumentEditWindow(PassportData passportData)
    {
        _documentData = passportData;
        _formBuilder = new DocumentFormBuilder(this);
        _dataUpdater = new DocumentDataUpdater(_documentData);
        _apiService = new ApiClient();

        InitializeWindowProperties();
        _formBuilder.BuildForm();
    }

    private void InitializeWindowProperties()
    {
        Text = "Данные отсканированного документа";
        Size = new Size(550, 850);
        MinimumSize = new Size(400, 600);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        TopMost = true;
        ShowInTaskbar = true;
    }

    public async Task SendDataAndCloseIfSuccess()
    {
        _dataUpdater.UpdateFromControls(_formBuilder.FieldControls);
        var success = await _apiService.SendPassportDataAsync(_documentData);
        if (success)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
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

    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);
        TopMost = true;
        Activate();
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_ACTIVATEAPP = 0x001C;
        
        if (m.Msg == WM_ACTIVATEAPP)
        {
            if (m.WParam != IntPtr.Zero)
            {
                TopMost = true;
            }
        }
        
        base.WndProc(ref m);
    }
}