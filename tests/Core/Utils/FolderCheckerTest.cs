using PassRegulaParser.Core.Utils;
namespace PassRegulaParser.Tests.Core.Utils;

public class FolderCheckerTests : IDisposable
{
    private const string TestFolder = "TestFolder123";
    private const string NonExistentFolder = "NonExistentFolder";

    public FolderCheckerTests()
    {
        Directory.CreateDirectory(TestFolder);
    }

    public void Dispose()
    {
        if (Directory.Exists(TestFolder))
        {
            Directory.Delete(TestFolder);
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void CheckIsFolderExists_WhenFolderExists_ReturnsTrueImmediately()
    {
        bool result = FolderChecker.IsFolderExists(TestFolder, 10);
        Assert.True(result);
    }

    [Fact]
    public void CheckIsFolderExists_WhenFolderDoesNotExist_ReturnsFalseAfterAllAttempts()
    {
        bool result = FolderChecker.IsFolderExists(NonExistentFolder, 10);
        Assert.False(result);
    }

    [Fact]
    public void CheckIsFolderExists_WhenFolderAppearsDuringRetries_ReturnsTrue()
    {
        Directory.Delete(TestFolder);
        Task.Run(() =>
        {
            Thread.Sleep(50);
            Directory.CreateDirectory(TestFolder);
        });

        bool result = FolderChecker.IsFolderExists(TestFolder, 100);

        Assert.True(result);
    }

    [Fact]
    public void CheckIsFolderExists_WhenPathIsInvalid_ReturnsFalse()
    {
        string invalidPath = "C:\\Invalid|Folder\\Name";
        bool result = FolderChecker.IsFolderExists(invalidPath, 20);
        Assert.False(result);
    }
}