using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace YashfeenMedical.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;
    private readonly string _signingKey;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _rootPath = configuration["FileStorage:LocalRootPath"] ?? "wwwroot/uploads";
        _signingKey = configuration["FileStorage:SigningKey"]
                      ?? throw new InvalidOperationException("FileStorage:SigningKey غير مُعرّف في الإعدادات");

        Directory.CreateDirectory(_rootPath);
    }

    public async Task<(string storedFileName, string relativePath)> SaveFileAsync(
        Stream fileStream, string originalFileName, string subFolder)
    {
        var extension = Path.GetExtension(originalFileName);
        var storedFileName = $"{Guid.NewGuid()}{extension}";

        var folderPath = Path.Combine(_rootPath, subFolder);
        Directory.CreateDirectory(folderPath);

        var fullPath = Path.Combine(folderPath, storedFileName);

        await using (var output = File.Create(fullPath))
        {
            await fileStream.CopyToAsync(output);
        }

        var relativePath = Path.Combine(subFolder, storedFileName).Replace("\\", "/");
        return (storedFileName, relativePath);
    }

    public string GenerateSignedUrl(string relativePath, TimeSpan validFor)
    {
        var expiry = DateTimeOffset.Now.Add(validFor).ToUnixTimeSeconds();
        var signature = ComputeSignature(relativePath, expiry);

        return $"/api/files/stream?path={Uri.EscapeDataString(relativePath)}&expires={expiry}&sig={signature}";
    }

    public bool ValidateSignedUrl(string relativePath, string signature, long expiryUnixSeconds)
    {
        if (DateTimeOffset.Now.ToUnixTimeSeconds() > expiryUnixSeconds)
            return false; // انتهت صلاحية الرابط

        var expectedSignature = ComputeSignature(relativePath, expiryUnixSeconds);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(signature),
            Encoding.UTF8.GetBytes(expectedSignature));
    }

    public void DeleteFile(string relativePath)
    {
        var fullPath = Path.Combine(_rootPath, relativePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public bool FileExists(string relativePath)
    {
        var fullPath = Path.Combine(_rootPath, relativePath);
        return File.Exists(fullPath);
    }

    public async Task MoveFileAsync(string relativeSourcePath, string relativeDestinationPath)
    {
        var sourceFull = Path.Combine(_rootPath, relativeSourcePath);
        var destFull = Path.Combine(_rootPath, relativeDestinationPath);

        if (!File.Exists(sourceFull))
            throw new FileNotFoundException("Source file not found", sourceFull);

        var destDir = Path.GetDirectoryName(destFull)!;
        Directory.CreateDirectory(destDir);

        // Use File.Move for an atomic move on same volume
        File.Move(sourceFull, destFull);

        await Task.CompletedTask;
    }

    private string ComputeSignature(string relativePath, long expiryUnixSeconds)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_signingKey));
        var payload = $"{relativePath}:{expiryUnixSeconds}";
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }

    public async Task<string> SaveProfilePhoto(IFormFile profilePhoto , string folderName)
    {
        string? profilePicturePath = null;
        FileValidationRules.Validate(profilePhoto, "ProfilePhoto");

        var (_, relativePath) = await SaveFileAsync(
            profilePhoto.OpenReadStream(),
            profilePhoto.FileName,
            subFolder: $"{folderName}/profile-pictures");

        profilePicturePath = relativePath;

        return profilePicturePath;
    }
}
