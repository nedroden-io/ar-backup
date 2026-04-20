using CommandLine;

namespace ArBackup;

public class Options
{
    [Option(HelpText = "Uploads a file to the storage")]
    public string Action { get; set; } = "upload";
    
    [Option('p', "path", Required = true, HelpText = "Path to the file to upload")]
    public string Path { get; set; } = string.Empty;
}