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
    }

    public void SaveAndClose()
    {
        _dataUpdater.UpdateFromControls(_formBuilder.FieldControls);
        DocumentDataSaver.SaveToJson(_documentData);
        DialogResult = DialogResult.OK;
        Close();
    }
}