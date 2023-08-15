using System.Security.Cryptography;
using System.Text;

namespace FileEncryptor;

public class Encryptor
{
    /// <summary>
    /// Encrypt the file of "inputFilePath" and output to the location of "outputFilePath".
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="outputFilePath"></param>
    /// <returns></returns>
    public static async Task Encrypt(string key, string inputFilePath, string outputFilePath)
    {
        using var inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
        using var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
        using var aes = Aes.Create();
        using var cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), aes.IV), CryptoStreamMode.Write);

        byte[] dummy = new byte[16];
        await cryptoStream.WriteAsync(dummy, 0, 16);

        byte[] buffer = new byte[8192];
        int len = 0;
        while((len = await inputFileStream.ReadAsync(buffer, 0, 8192)) > 0)
        {
            await cryptoStream.WriteAsync(buffer, 0, len);
        }
    }

    /// <summary>
    /// Decrypts the encrypted file at "inputFilePath" and returns it as a byte array.
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <returns></returns>
    public static async Task<byte[]> Decrypt(string key, string inputFilePath)
    {
        using var aes = Aes.Create();
        using var inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), aes.IV), CryptoStreamMode.Read);

        byte[] dummy = new byte[16];
        await cryptoStream.ReadAsync(dummy, 0, 16);

        byte[] buffer = new byte[8192];
        int len = 0;
        while ((len = await cryptoStream.ReadAsync(buffer, 0, 8192)) > 0)
        {
            await memoryStream.WriteAsync(buffer, 0, len);
            await memoryStream.FlushAsync();
        }

        return memoryStream.ToArray();
    }
}