# FileEncryptor
A library for encrypting and decrypting files. (recommended to modify)

- C#
- Net 7.0

## How to use!!

### Encrypt(console command)
#### Step.1
Copy the Library folder in Repository into the project directory that uses it.
#### Step.2
Use the following console command.
```console
dotnet {your workspace directory}/Library/FileEncryptor/FileEncryptor.dll {input directory} {output directory} {key} {file extension}
```
#### Tips
- All files including subfiles in {input directory} will be encrypted and output to {output directory}.
- Enter a 32-character random number for the key.
- The key must match during encryption and decryption.

### Encrypt(used in code)
#### Step.1
Copy the Library folder in Repository into the project directory that uses it.
#### Step.2
Add the following lines to your project file (.csproj) that uses FileEncryptor.
```csproj
<ItemGroup>
  <Reference Include="FileEncryptor">
    <SpecificVersion>False</SpecificVersion>
    <HintPath>Library\FileEncryptor\FileEncryptor.dll</HintPath>
  </Reference>
</ItemGroup>
```
#### Step.3
Use FileEncryptor.Encrypt() within any script.
```C#
foreach (var file in inputDirectoryInfo.GetFiles("*", SearchOption.AllDirectories))
{
    var outputFilePath = file.FullName.Replace(inputDirectoryPath, outputDirectoryPath);
    Encryptor.Encrypt(KEY, filefile.FullName, outputFilePath);
}
```

### Decrypt
#### Step.1
Copy the Library folder in Repository into the project directory that uses it.
#### Step.2
Add the following lines to your project file (.csproj) that uses FileEncryptor.
```csproj
<ItemGroup>
  <Reference Include="FileEncryptor">
    <SpecificVersion>False</SpecificVersion>
    <HintPath>Library\FileEncryptor\FileEncryptor.dll</HintPath>
  </Reference>
</ItemGroup>
```
#### Step.3
Use FileEncryptor.Encrypt() within any script.
```C#
foreach (var file in inputDirectoryInfo.GetFiles("*", SearchOption.AllDirectories))
{
    // Decrypt.
    var fileByte = Encryptor.Decrypt(KEY, filefile.FullName);
    // Create texture.
    var texture = new Texture(fileByte);
}
```

### Error...
If the key used for encryption is not a 32-character number, the following error will occur even if the key matches during decryption.
```
ERROR!!
Specified key is not a valid size for this algorithm. (Parameter 'rgbKey')
```
