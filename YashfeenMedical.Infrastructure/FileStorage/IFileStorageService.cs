namespace YashfeenMedical.Infrastructure.FileStorage;

public interface IFileStorageService
{
    // يحفظ الملف ويعيد الاسم المخزّن (GUID + امتداد) + المسار النسبي
    Task<(string storedFileName, string relativePath)> SaveFileAsync(Stream fileStream, string originalFileName, string subFolder);

    // يولّد رابط تحميل مؤقت وموقّع (Signed URL) بدل الروابط المباشرة
    string GenerateSignedUrl(string relativePath, TimeSpan validFor);

    bool ValidateSignedUrl(string relativePath, string signature, long expiryUnixSeconds);

    void DeleteFile(string relativePath);
}
