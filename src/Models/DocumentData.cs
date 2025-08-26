namespace PassRegulaParser.Models;

public abstract class DocumentData
{
    public string? DocumentType { get; set; }
    public string? Description { get; set; }
    public string DataSaveAgreement { get; set; } = "False";
    public string DataSaveAgreementDateEnd { get; set; } = "30.01.2026";

     public override string ToString() =>
    $@"Document Type: {DocumentType}
    DataSaveAgreement: {DataSaveAgreement}
    DataSaveAgreementDateEnd: {DataSaveAgreementDateEnd}
    Description: {Description}";
}