namespace PassRegulaParser.Models;

public abstract class DocumentData
{
    public string? DocumentType { get; set; }
    public string? Description { get; set; }
    public string DataSaveAgreement { get; set; } = "0";
    public string DataSaveAgreementDateEnd { get; set; }
    public string? Username { get; set; }

    protected DocumentData()
    {
        DataSaveAgreementDateEnd = DateTime.Today.ToString("dd.MM.yyyy");
    }

    public override string ToString() =>
   $@"Document Type: {DocumentType}
    DataSaveAgreement: {DataSaveAgreement}
    DataSaveAgreementDateEnd: {DataSaveAgreementDateEnd}
    Description: {Description}
    Username: {Username}";
}