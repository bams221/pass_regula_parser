namespace PassRegulaParser.Core.Services;

public class TrayIconService : IDisposable
{
    private readonly NotifyIcon _notifyIcon;
    private readonly SynchronizationContext _syncContext;
    private bool _isDisposed;

    public TrayIconService(string appName, Action onDoubleClick)
    {
        _syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

        _notifyIcon = new NotifyIcon
        {
            Icon = TryLoadIcon("icon.ico") ?? SystemIcons.Application,
            Text = appName,
            Visible = true,
            ContextMenuStrip = CreateContextMenu()
        };

        _notifyIcon.DoubleClick += (s, e) => _syncContext.Post(_ => onDoubleClick(), null);
    }

    internal static Icon? TryLoadIcon(string path)
    {
        try
        {
            if (File.Exists(path))
                return new Icon(path);
        }
        catch { /* Ignore */ }
        return null;
    }

    internal ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();

        var exitItem = new ToolStripMenuItem("Выход");
        exitItem.Click += OnExitClicked;
        menu.Items.Add(exitItem);
        return menu;
    }

    internal void OnExitClicked(object? sender, EventArgs e)
    {
        _syncContext.Post(_ =>
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            Environment.Exit(0);
        }, null);
    }

    public void ShowNotification(string title, string message, ToolTipIcon icon = ToolTipIcon.Info)
    {
        _syncContext.Post(_ =>
           {
               _notifyIcon.ShowBalloonTip(3000, title, message, icon);
           }, null);
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _syncContext.Post(_ =>
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }, null);

        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}