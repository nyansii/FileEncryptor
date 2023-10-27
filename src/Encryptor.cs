using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FileEncryptor;

public class Encryptor
{
	/// <summary>
	/// Encrypt the file of "inputFilePath" and output to the location of "outputFilePath".
	/// </summary>
	/// <param name="inputFilePath"></param>
	/// <param name="outputFilePath"></param>
	/// <returns>Returns true if the encryption was successful.</returns>
	public static bool Encrypt(string keyFile, string inputFilePath, string outputFilePath)
	{
		try
		{
			if (!Path.Exists(keyFile))
			{
				Console.WriteLine("ERROR!!");
				Console.WriteLine("keyFile not found.");
				return false;
			}

			string jsonString = File.ReadAllText(keyFile);
			Key? key = JsonSerializer.Deserialize<Key>(jsonString);
			if (key == null)
			{
				Console.WriteLine("ERROR!!");
				Console.WriteLine("key is null.");
				return false;
			}
			using var inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
			using var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
			using var aes = Aes.Create();
			using var cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(Encoding.UTF8.GetBytes(key.Value), aes.IV), CryptoStreamMode.Write);

			byte[] dummy = new byte[16];
			cryptoStream.Write(dummy, 0, 16);

			byte[] buffer = new byte[8192];
			int len = 0;
			while ((len = inputFileStream.Read(buffer, 0, 8192)) > 0)
			{
				cryptoStream.Write(buffer, 0, len);
			}

			return true;
		}
		catch (Exception e)
		{
			Console.WriteLine("ERROR!!");
			Console.WriteLine(e.Message);
			Console.WriteLine(e.StackTrace);
			return false;
		}
	}

	/// <summary>
	/// Decrypts the encrypted file at "inputFilePath" and returns it as a byte array.
	/// </summary>
	/// <param name="inputFilePath"></param>
	/// <returns></returns>
	public static byte[]? Decrypt(string key, string inputFilePath)
	{
		try
		{
			using var aes = Aes.Create();
			using var inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
			using var memoryStream = new MemoryStream();
			using var cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), aes.IV), CryptoStreamMode.Read);

			byte[] dummy = new byte[16];
			cryptoStream.Read(dummy, 0, 16);

			byte[] buffer = new byte[8192];
			int len = 0;
			while ((len = cryptoStream.Read(buffer, 0, 8192)) > 0)
			{
				memoryStream.Write(buffer, 0, len);
				memoryStream.FlushAsync();
			}

			return memoryStream.ToArray();
		}
		catch (Exception e)
		{
			Console.WriteLine("ERROR!!");
			Console.WriteLine(e.Message);
			Console.WriteLine(e.StackTrace);
			return null;
		}
	}

	public class Key
	{
		public string Value { get; set; } = "";
	}
}
