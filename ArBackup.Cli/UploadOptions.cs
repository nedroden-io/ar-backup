using CommandLine;

namespace ArBackup;

[Verb("upload", HelpText = "Uploads a file to the storage")]
public class UploadOptions
{
    [Option('p', "path", Required = true, HelpText = "Path to the file to upload")]
    public string Path { get; set; } = string.Empty;
}

[Verb("restore", HelpText = "Restores a file from storage")]
public class RestoreOptions
{
    [Option('f', "file", Required = true, HelpText = "File to restore")]
    public string File { get; set; } = string.Empty;
}