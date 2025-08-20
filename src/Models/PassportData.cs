namespace PassRegulaParser.Models;

public class PassportData : DocumentData
{
    public string? FullName { get; set; }
    public string? SerialNumber { get; set; }
    public string? BirthCity { get; set; }
    public string? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? IssueDate { get; set; }
    public string? Authority { get; set; }
    public string? AuthorityCode { get; set; }
    public string? PhotoBase64 { get; set; }

    public override string ToString() =>
    $@"Passport Data:
    Document Type: {DocumentType}
    Full Name: {FullName}
    Serial Number: {SerialNumber}
    Birth City: {BirthCity}
    Birth Date: {BirthDate}
    Gender: {Gender}
    Issue Date: {IssueDate}
    Authority: {Authority}
    Authority Code: {AuthorityCode}
    Description: {Description}";
}