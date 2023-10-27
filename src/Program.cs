namespace FileEncryptor;

class Program
{
	static async Task Main(string[] args)
	{
		try
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
				Console.WriteLine(inputDirectoryPath);
				return;
			}

			if (!Directory.Exists(outputDirectoryPath))
			{
				Directory.CreateDirectory(outputDirectoryPath);
			}

			var keyFileName = args[2];
			var fileExtension = args[3];

			if (keyFileName == "")
			{
				Console.WriteLine("ERROR.");
				Console.WriteLine("argument 3(keyFileName): Name is empty.");
				return;
			}

			if (fileExtension == "")
			{
				Console.WriteLine("ERROR.");
				Console.WriteLine("argument 4(fileExtension): FileExtension is empty.");
				return;
			}

			inputDirectoryPath = inputDirectoryPath.Replace("/", @"\");
			outputDirectoryPath = outputDirectoryPath.Replace("/", @"\");

			// All files existing in the directory of Path passed in argument 1 are encrypted into *.asset files and output to the position of Path of argument 2.
			foreach (var file in new DirectoryInfo(inputDirectoryPath).GetFiles("*", SearchOption.AllDirectories))
			{
				if (file.FullName.IndexOf(inputDirectoryPath) == -1) continue;

				// Get output file path.
				var outputFilePath = file.FullName.Replace(inputDirectoryPath, outputDirectoryPath);
				outputFilePath = outputFilePath.Remove(outputFilePath.LastIndexOf("."), outputFilePath.Length - outputFilePath.LastIndexOf(".")) + $".{fileExtension}";

				// Get output directory.
				var outputDirectory = outputFilePath.Remove(outputFilePath.LastIndexOf(@"\"), outputFilePath.Length - outputFilePath.LastIndexOf(@"\"));

				// Create directory.
				if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

				// File encrypt.
				var result = await Task.Factory.StartNew(() =>
				{
					return Encryptor.Encrypt(keyFileName, file.FullName, outputFilePath);
				});

				if (!result)
				{
					Console.WriteLine("Encryption did not complete successfully due to an error.");
					return;
				}
			}

			Console.WriteLine("Encryption successfully completed.");
		}
		catch
		{
			throw;
		}
	}
}
