using PassRegulaParser.Core.Services;
using PassRegulaParser.Core.Handlers;
using Microsoft.Extensions.Configuration;

namespace PassRegulaParser;

class ServiceRunner
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        string pathToWatch = configuration["PathToWatch"] ?? throw new FormatException("PathToWatch is missing in appsettings.json config file");
        string fileFilter = configuration["FileFilter"] ?? throw new FormatException("FileFilter is missing in appsettings.json config file");
        
        DocumentRecognitionCoordinator newRecognitionHandler = new(pathToWatch);
        DirWatcherService watcher = new(
            pathToWatch,
            fileFilter,
            newRecognitionHandler.OnChangeDetected);

        watcher.Start();
    }
}