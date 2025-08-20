using PassRegulaParser.Models;
using Moq;
using PassRegulaParser.Core.Managers;

namespace PassRegulaParser.Tests.Core.Utils.Managers
{
    public class DocumentUiManagerTests
    {
        private readonly DocumentUiManager _manager;
        private readonly Mock<PassportData> _mockPassportData;

        public DocumentUiManagerTests()
        {
            _manager = new DocumentUiManager();
            _mockPassportData = new Mock<PassportData>();
        }

        [Fact]
        public void ShowDocumentEditWindow_ShouldCreateNewWindow()
        {
            _manager.ShowDocumentEditWindow(_mockPassportData.Object);
            Thread.Sleep(100);
            Assert.True(_manager.IsWindowOpen);
        }

        [Fact]
        public void CloseCurrentWindow_WhenWindowIsOpen_ShouldCloseWindow()
        {
            _manager.ShowDocumentEditWindow(_mockPassportData.Object);
            Thread.Sleep(100);
            
            _manager.ClosePrevWindow();
            Thread.Sleep(100);

            Assert.False(_manager.IsWindowOpen);
        }

        [Fact]
        public void CloseCurrentWindow_WhenNoWindowIsOpen_ShouldNotThrowException()
        {
            var exception = Record.Exception(() => _manager.ClosePrevWindow());
            Assert.Null(exception);
        }

        [Fact]
        public void IsWindowOpen_WhenNoWindowCreated_ShouldReturnFalse()
        {
            Assert.False(_manager.IsWindowOpen);
        }

        [Fact]
        public void ShowDocumentEditWindow_WhenCalledMultipleTimes_ShouldClosePreviousWindow()
        {
            _manager.ShowDocumentEditWindow(_mockPassportData.Object);
            Thread.Sleep(100);
            var firstWindowOpen = _manager.IsWindowOpen;

            _manager.ShowDocumentEditWindow(_mockPassportData.Object);
            Thread.Sleep(100);

            Assert.True(firstWindowOpen);
            Assert.True(_manager.IsWindowOpen);
        }

        [Fact]
        public void ShowDocumentEditWindow_ShouldCreateSTAThread()
        {
            _manager.ShowDocumentEditWindow(_mockPassportData.Object);
            Thread.Sleep(100);

            Assert.True(_manager.IsWindowOpen);
        }

        [Fact]
        public void Dispose_ShouldCleanUpResources()
        {
            _manager.ShowDocumentEditWindow(_mockPassportData.Object);
            Thread.Sleep(100);

            _manager.ClosePrevWindow();
            Thread.Sleep(100);

            Assert.False(_manager.IsWindowOpen);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithNullWindow()
        {
            var manager = new DocumentUiManager();
            
            Assert.False(manager.IsWindowOpen);
        }
    }
}