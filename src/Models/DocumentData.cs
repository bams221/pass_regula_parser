namespace PassRegulaParser.Models;

public abstract class DocumentData
{
    public string? DocumentType { get; set; }
    public string? Description { get; set; }
    public string DataSaveAgreement { get; set; } = "False";
    public string DataSaveAgreementDateEnd { get; set; }

    protected DocumentData()
    {
        DataSaveAgreementDateEnd = DateTime.Today.AddYears(1).ToString("dd.MM.yyyy");
    }

    public override string ToString() =>
   $@"Document Type: {DocumentType}
    DataSaveAgreement: {DataSaveAgreement}
    DataSaveAgreementDateEnd: {DataSaveAgreementDateEnd}
    Description: {Description}";
}