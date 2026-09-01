namespace YashfeenMedical.Infrastructure.FileStorage;

// حدود الحجم والامتدادات المسموحة حسب نوع الملف (راجع القسم 7.1 من متطلبات المشروع)
public static class FileValidationRules
{
    public static readonly Dictionary<string, (string[] AllowedExtensions, long MaxSizeKB)> Rules = new()
    {
        ["XRay"] = (new[] { ".jpg", ".jpeg", ".png", ".dcm" }, 20 * 1024),
        ["Report"] = (new[] { ".pdf" }, 10 * 1024),
        ["ProfilePhoto"] = (new[] { ".jpg", ".jpeg", ".png" }, 2 * 1024),
        ["ConsentForm"] = (new[] { ".pdf" }, 5 * 1024),
        ["Prescription"] = (new[] { ".pdf", ".jpg", ".jpeg", ".png" }, 5 * 1024)
    };
}
