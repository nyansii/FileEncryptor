namespace FileEncryptor;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: FileEncryptor.dll <inputDirectoryPath> <outputDirectoryPath> <key> <fileExtension>");
            return;
        }

        var inputDirectoryPath = args[0];
        var outputDirectoryPath = args[1];

        if (!Directory.Exists(inputDirectoryPath))
        {
            Console.WriteLine("ERROR.");
            Console.WriteLine("argument 1(inputPath): Directory not found.");
            return;
        }

        if (!Directory.Exists(outputDirectoryPath))
        {
            Console.WriteLine("ERROR.");
            Console.WriteLine("argument 2(outputPath): Directory not found.");
            return;
        }

        var key = args[2];
        var fileExtension = args[3];

        if (key != "")
        {
            Console.WriteLine("ERROR.");
            Console.WriteLine("argument 3(key): key is empty.");
        }

        if (fileExtension != "")
        {
            Console.WriteLine("ERROR.");
            Console.WriteLine("argument 3(key): fileExtension is empty.");
        }

        // All files existing in the directory of Path passed in argument 1 are encrypted into *.asset files and output to the position of Path of argument 2.
        foreach (var file in new DirectoryInfo(inputDirectoryPath).GetFiles("*", SearchOption.AllDirectories))
        {
            // Get output file path.
            var outputFilePath = file.FullName.Replace(inputDirectoryPath, outputDirectoryPath);
            outputFilePath = outputFilePath.Remove(outputFilePath.LastIndexOf("."), outputFilePath.Length - outputFilePath.LastIndexOf(".")) + $".{fileExtension}";

            // Get output directory.
            var outputDirectory = outputFilePath.Remove(outputFilePath.LastIndexOf(@"\"), outputFilePath.Length - outputFilePath.LastIndexOf(@"\"));

            // Create directory.
            if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

            // File encrypt.
            await Encryptor.Encrypt(key, file.FullName, outputFilePath);
        }
    }
}
