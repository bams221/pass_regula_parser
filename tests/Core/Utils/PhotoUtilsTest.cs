using PassRegulaParser.Core.Utils;

namespace PassRegulaParser.Tests.Core.Utils;

public class PhotoUtilsTests : IDisposable
{
    private const string TestFilesDirectory = "TestFiles";
    private const string ValidJpgFile = "test.jpg";
    private const string ValidJpegFile = "test.jpeg";
    private const string InvalidExtensionFile = "test.png";
    private const string NonExistentFile = "nonexistent.jpg";

    public PhotoUtilsTests()
    {
        Directory.CreateDirectory(TestFilesDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(TestFilesDirectory))
        {
            Directory.Delete(TestFilesDirectory, true);
        }
    }

    [Fact]
    public void GetPhotoBase64FromFile_WhenFileDoesNotExist_ReturnsEmptyString()
    {
        var filePath = Path.Combine(TestFilesDirectory, NonExistentFile);

        var result = PhotoUtils.GetPhotoBase64FromFile(filePath);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetPhotoBase64FromFile_WhenFileHasInvalidExtension_ReturnsEmptyString()
    {
        var filePath = Path.Combine(TestFilesDirectory, InvalidExtensionFile);
        File.WriteAllBytes(filePath, [0x01, 0x02, 0x03]);

        var result = PhotoUtils.GetPhotoBase64FromFile(filePath);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetPhotoBase64FromFile_WhenValidJpgFile_ReturnsBase64String()
    {
        var filePath = Path.Combine(TestFilesDirectory, ValidJpgFile);
        var testImageBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46 };
        File.WriteAllBytes(filePath, testImageBytes);

        var result = PhotoUtils.GetPhotoBase64FromFile(filePath);

        Assert.NotEqual(string.Empty, result);
        Assert.Equal(Convert.ToBase64String(testImageBytes), result);
    }

    [Fact]
    public void GetPhotoBase64FromFile_WhenValidJpegFile_ReturnsBase64String()
    {
        var filePath = Path.Combine(TestFilesDirectory, ValidJpegFile);
        var testImageBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46 };
        File.WriteAllBytes(filePath, testImageBytes);

        var result = PhotoUtils.GetPhotoBase64FromFile(filePath);

        Assert.NotEqual(string.Empty, result);
        Assert.Equal(Convert.ToBase64String(testImageBytes), result);
    }

    [Fact]
    public void GetPhotoBase64FromFile_WhenEmptyJpgFile_ReturnsEmptyBase64String()
    {
        var filePath = Path.Combine(TestFilesDirectory, "empty.jpg");
        File.WriteAllBytes(filePath, Array.Empty<byte>());

        var result = PhotoUtils.GetPhotoBase64FromFile(filePath);

        Assert.Equal(Convert.ToBase64String(Array.Empty<byte>()), result);
    }
}