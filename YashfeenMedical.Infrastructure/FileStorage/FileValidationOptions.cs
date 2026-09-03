using Microsoft.AspNetCore.Http;
using YashfeenMedical.Infrastructure.Exceptions;

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

    public static void Validate(IFormFile file, string category)
    {
        if (!Rules.TryGetValue(category, out var rule))
            throw new ArgumentException($"the category '{category}' is not supported");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!rule.AllowedExtensions.Contains(extension))
            throw new BadRequestException(
                $"amtidad al-malf '{extension}' ghayr masmuch. al-amtidadat al-masmucha li-{category}: {string.Join(", ", rule.AllowedExtensions)}");

        var fileSizeKB = file.Length / 1024;
        if (fileSizeKB > rule.MaxSizeKB)
            throw new BadRequestException(
               $"File size ({fileSizeKB} KB) exceeds the maximum allowed size ({rule.MaxSizeKB} KB) for {category}.");
    }
}
