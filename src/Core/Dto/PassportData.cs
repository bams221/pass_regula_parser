namespace PassRegulaParser.Core.Dto;

public class PassportData
{
    public string? DocumentType { get; set; }
    public string? FullName { get; set; }
    public string? SerialNumber { get; set; }
    public string? BirthCity { get; set; }
    public string? BirthDate { get; set; }
    public string? Gender { get; set; }

    public override string ToString() =>
        $"Type: {DocumentType}, FullName: {FullName}, SerialNumber: {SerialNumber}, BirthCity: {BirthCity}, BirthDate: {BirthDate}, Gender: {Gender}";
}