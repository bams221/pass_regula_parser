using System.Windows.Forms;
using Moq;
using PassRegulaParser.Core.Services;

namespace PassRegulaParser.Tests.Core.Services;

public class TrayIconServiceTests : IDisposable
{
    private readonly Mock<SynchronizationContext> _mockSyncContext;
    private readonly string _testIconPath = "test_icon.ico";
    private readonly string _appName = "TestApp";

    public TrayIconServiceTests()
    {
        _mockSyncContext = new Mock<SynchronizationContext>();
        SynchronizationContext.SetSynchronizationContext(_mockSyncContext.Object);
    }

    public void Dispose()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void TryLoadIcon_ReturnsIcon_IfFileExists()
    {
        File.WriteAllBytes(_testIconPath, []);

        var icon = TrayIconService.TryLoadIcon(_testIconPath);

        Assert.Null(icon);
        File.Delete(_testIconPath);
    }

    [Fact]
    public void TryLoadIcon_ReturnsNull_IfFileDoesNotExist()
    {
        var icon = TrayIconService.TryLoadIcon("nonexistent.ico");

        Assert.Null(icon);
    }

    [Fact]
    public void CreateContextMenu_CreatesMenuWithExitItem()
    {
        var service = new TrayIconService(_appName, () => { });

        var menu = service.CreateContextMenu();

        Assert.Single(menu.Items);
        Assert.Equal("Выход", ((ToolStripMenuItem)menu.Items[0]).Text);
    }
}
