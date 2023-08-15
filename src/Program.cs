namespace FileEncryptor;

class Program
{
    // you edit.-------------------------------------------
    //
    // Key required for file encryption and decryption.
    private const string KEY = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    // File extension after encryption.
    private const string FILE_EXTENSION = "asset";
    //
    // ----------------------------------------------------

    static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: FileEncryptor.dll <inputDirectoryPath> <outputDirectoryPath>");
            return;
        }

        var inputDirectoryPath = args[0];
        var outputDirectoryPath = args[1];

        if (!Directory.Exists(inputDirectoryPath))
        {
            Console.WriteLine("ERROR.");
            Console.WriteLine("argument 0(inputPath): Directory not found.");
            return;
        }

        if (!Directory.Exists(outputDirectoryPath))
        {
            Console.WriteLine("ERROR.");
            Console.WriteLine("argument 0(outputPath): Directory not found.");
            return;
        }

        // All files existing in the directory of Path passed in argument 1 are encrypted into *.asset files and output to the position of Path of argument 2.
        foreach (var file in new DirectoryInfo(inputDirectoryPath).GetFiles("*", SearchOption.AllDirectories))
        {
            // Get output file path.
            var outputFilePath = file.FullName.Replace(inputDirectoryPath, outputDirectoryPath);
            outputFilePath = outputFilePath.Remove(outputFilePath.LastIndexOf("."), outputFilePath.Length - outputFilePath.LastIndexOf(".")) + $".{FILE_EXTENSION}";

            // Get output directory.
            var outputDirectory = outputFilePath.Remove(outputFilePath.LastIndexOf(@"\"), outputFilePath.Length - outputFilePath.LastIndexOf(@"\"));

            // Create directory.
            if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

            // File encrypt.
            await Encryptor.Encrypt(KEY, file.FullName, outputFilePath);
        }
    }
}
