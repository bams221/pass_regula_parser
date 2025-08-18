using System.Text.Json.Nodes;
using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Tests.Core.Utils;

public class FieldListTests
{
    [Fact]
    public void Constructor_WithNullFieldArray_ThrowsArgumentNullException()
    {
        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() =>
            new FieldList(null, "value", "name"));
        #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void GetValue_FieldExists_ReturnsCorrectValue()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Document Number"", ""value"": ""123456""},
                {""fieldName"": ""Date of Birth"", ""value"": ""01.01.1990""}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName");

        // Act
        var result = fieldList.GetValue("Document Number");

        // Assert
        Assert.Equal("123456", result);
    }

    [Fact]
    public void GetValue_FieldDoesNotExist_ReturnsEmptyString()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Document Number"", ""value"": ""123456""}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName");

        // Act
        var result = fieldList.GetValue("Non-existent Field");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetValue_FieldExistsButValueIsNull_ReturnsEmptyString()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Document Number"", ""value"": null}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName");

        // Act
        var result = fieldList.GetValue("Document Number");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetValueRussian_WithLanguageProperties_ReturnsCorrectValue()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Surname And Given Names"", ""lcidName"": ""Russian"", ""value"": ""Иванов Иван Иванович""},
                {""fieldName"": ""Surname And Given Names"", ""lcidName"": ""English"", ""value"": ""Ivanov Ivan Ivanovich""}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName", "lcidName", "Russian");

        // Act
        var result = fieldList.GetValueRussian("Surname And Given Names");

        // Assert
        Assert.Equal("Иванов Иван Иванович", result);
    }

    [Fact]
    public void GetValueRussian_WithoutLanguageProperties_ThrowsInvalidOperationException()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Surname And Given Names"", ""value"": ""Иванов Иван Иванович""}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            fieldList.GetValueRussian("Surname And Given Names"));
    }

    [Fact]
    public void GetValueRussian_FieldExistsButWrongLanguage_ReturnsEmptyString()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Surname And Given Names"", ""lcidName"": ""English"", ""value"": ""Ivanov Ivan Ivanovich""}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName", "lcidName", "Russian");

        // Act
        var result = fieldList.GetValueRussian("Surname And Given Names");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetValueRussian_FieldExistsButLanguagePropertyIsNull_ReturnsEmptyString()
    {
        // Arrange
        var json = @"[
                {""fieldName"": ""Surname And Given Names"", ""lcidName"": null, ""value"": ""Иванов Иван Иванович""}
            ]";
        var fieldArray = JsonNode.Parse(json)?.AsArray();
        var fieldList = new FieldList(fieldArray!, "value", "fieldName", "lcidName", "Russian");

        // Act
        var result = fieldList.GetValueRussian("Surname And Given Names");

        // Assert
        Assert.Equal(string.Empty, result);
    }
}