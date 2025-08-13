namespace PassRegulaParser.Core.Dto;

public class PassportData
{
    public string? DocumentType { get; set; }
    public string? FullName { get; set; }

    public override string ToString() =>
        $"Type: {DocumentType}, FullName: {FullName}";
}