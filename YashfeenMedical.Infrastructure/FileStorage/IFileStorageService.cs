using Microsoft.AspNetCore.Http;

namespace YashfeenMedical.Infrastructure.FileStorage;

public interface IFileStorageService
{
    // يحفظ الملف ويعيد الاسم المخزّن (GUID + امتداد) + المسار النسبي
    Task<(string storedFileName, string relativePath)> SaveFileAsync(Stream fileStream, string originalFileName, string subFolder);

    // يولّد رابط تحميل مؤقت وموقّع (Signed URL) بدل الروابط المباشرة
    string GenerateSignedUrl(string relativePath, TimeSpan validFor);

    bool ValidateSignedUrl(string relativePath, string signature, long expiryUnixSeconds);

    void DeleteFile(string relativePath);

    Task<string> SaveProfilePhoto(IFormFile profilePhoto, string folderName);

    // Move a file from one relative path to another within the storage root.
    // Throws if source does not exist or move fails.
    Task MoveFileAsync(string relativeSourcePath, string relativeDestinationPath);

    // Return true if file exists at the given relative path.
    bool FileExists(string relativePath);
}
