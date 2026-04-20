using CommandLine;

namespace ArBackup;

public class Options
{
    [Option(HelpText = "Uploads a file to the storage")]
    public string Action { get; set; } = "upload";
    
    [Option('p', "path", HelpText = "Path to the file to upload")]
    public string Path { get; set; } = string.Empty;

    [Option('f', "file", HelpText = "File to restore")]
    public Guid File { get; set; } = Guid.NewGuid();
}