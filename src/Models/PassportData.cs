namespace PassRegulaParser.Models;

public class PassportData : DocumentData
{
    public string? FullName { get; set; }
    public string? Serial { get; set; }
    public string? Number { get; set; }
    public string? BirthCity { get; set; }
    public string? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? IssueDate { get; set; }
    public string? Authority { get; set; }
    public string? AuthorityCode { get; set; }
    public string? PhotoBase64 { get; set; }

    public PassportData Clone()
    {
        return new PassportData
        {
            DocumentType = DocumentType,
            Description = Description,
            DataSaveAgreement = DataSaveAgreement,
            DataSaveAgreementDateEnd = DataSaveAgreementDateEnd,

            FullName = FullName,
            Serial = Serial,
            Number = Number,
            BirthCity = BirthCity,
            BirthDate = BirthDate,
            Gender = Gender,
            IssueDate = IssueDate,
            Authority = Authority,
            AuthorityCode = AuthorityCode,
            PhotoBase64 = PhotoBase64
        };
    }


    public override string ToString() =>
    base.ToString() +
    $@"
    Full Name: {FullName}
    Serial: {Serial}
    Number: {Number}
    Birth City: {BirthCity}
    Birth Date: {BirthDate}
    Gender: {Gender}
    Issue Date: {IssueDate}
    Authority: {Authority}
    Authority Code: {AuthorityCode};";
}