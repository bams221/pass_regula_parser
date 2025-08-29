using PassRegulaParser.Core.Services;
using PassRegulaParser.Core.Handlers;
using Microsoft.Extensions.Configuration;
using PassRegulaParser.Config;

namespace PassRegulaParser;

class ServiceRunner
{
    public static void Main(string[] args)
    {
        string pathToWatch = AppConfiguration.Configuration["PathToWatch"]
            ?? throw new FormatException("PathToWatch is missing in appsettings.json config file");
        string fileFilter = AppConfiguration.Configuration["FileFilter"]
            ?? throw new FormatException("FileFilter is missing in appsettings.json config file");

        
        DocumentRecognitionCoordinator newRecognitionHandler = new(pathToWatch);
        DirWatcherService watcher = new(
            pathToWatch,
            fileFilter,
            newRecognitionHandler.OnChangeDetected);

        watcher.Start();
    }
}