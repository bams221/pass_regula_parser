using PassRegulaParser.Models;

namespace PassRegulaParser.Tests.Models;

public class PassportDataTests
{
    [Fact]
    public void Clone_ShouldCreateDeepCopy_WithAllProperties()
    {
        var original = new PassportData
        {
            DocumentType = "Passport",
            Note = "Test passport",
            DataSaveAgreement = "1",
            DataSaveAgreementDateEnd = "12.31.2024",
            FullName = "Иванов Иван Иванович",
            Serial = "45 01",
            Number = "123456",
            BirthPlace = "Москва",
            BirthDate = "01.01.1990",
            Gender = "М",
            IssueDate = "01.01.2020",
            Authority = "ОУФМС",
            AuthorityCode = "770-001",
            PhotoBase64 = "base64string"
        };

        var cloned = original.Clone();

        Assert.NotSame(original, cloned);
        Assert.Equal(original.DocumentType, cloned.DocumentType);
        Assert.Equal(original.Note, cloned.Note);
        Assert.Equal(original.DataSaveAgreement, cloned.DataSaveAgreement);
        Assert.Equal(original.DataSaveAgreementDateEnd, cloned.DataSaveAgreementDateEnd);
        Assert.Equal(original.FullName, cloned.FullName);
        Assert.Equal(original.Serial, cloned.Serial);
        Assert.Equal(original.Number, cloned.Number);
        Assert.Equal(original.BirthPlace, cloned.BirthPlace);
        Assert.Equal(original.BirthDate, cloned.BirthDate);
        Assert.Equal(original.Gender, cloned.Gender);
        Assert.Equal(original.IssueDate, cloned.IssueDate);
        Assert.Equal(original.Authority, cloned.Authority);
        Assert.Equal(original.AuthorityCode, cloned.AuthorityCode);
        Assert.Equal(original.PhotoBase64, cloned.PhotoBase64);
    }

    [Fact]
    public void Clone_ShouldHandleNullProperties()
    {
        var original = new PassportData
        {
            FullName = null,
            Serial = null,
            Gender = null
        };

        var cloned = original.Clone();

        Assert.Null(cloned.FullName);
        Assert.Null(cloned.Serial);
        Assert.Null(cloned.Gender);
    }
}