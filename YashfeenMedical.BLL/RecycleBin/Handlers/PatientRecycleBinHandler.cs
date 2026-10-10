using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.Infrastructure.FileStorage;
using YashfeenMedical.Infrastructure.UsersManagment;

namespace YashfeenMedical.BLL.RecycleBin.Handlers
{
    public class PatientRecycleBinHandler : IRecycleBinHandler
    {
        public string EntityKey => "patient";

        private readonly IFileStorageService _fileStorage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagmentServices _userManagement;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientRecycleBinHandler(IFileStorageService fileStorage, IUnitOfWork unitOfWork,
            IUserManagmentServices userManagement, UserManager<ApplicationUser> userManager)
        {
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
            _userManagement = userManagement;
            _userManager = userManager;
        }

        public async Task OnSoftDeleteAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is not Patient patient) return;

            if (string.IsNullOrWhiteSpace(patient.ProfilePhotoUrl)) return;

            var fileName = Path.GetFileName(patient.ProfilePhotoUrl);
            var destRelative = Path.Combine("recycle-bin", "patients", patient.Id.ToString(), fileName).Replace("\\", "/");

            try
            {
                await _fileStorage.MoveFileAsync(patient.ProfilePhotoUrl, destRelative);
                patient.ProfilePhotoUrl = destRelative;
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                // Let exceptions bubble to be logged by middleware; do not rethrow to avoid breaking delete
            }
        }

        public async Task OnRestoreAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is not Patient patient) return;

            if (string.IsNullOrWhiteSpace(patient.ProfilePhotoUrl)) return;

            // assume recycle-bin path contains /recycle-bin/patients/{id}/filename
            var fileName = Path.GetFileName(patient.ProfilePhotoUrl);
            var originalRelative = Path.Combine("patients", "profile-pictures", fileName).Replace("\\", "/");

            try
            {
                if (_fileStorage.FileExists(patient.ProfilePhotoUrl))
                {
                    // if destination exists, generate unique name
                    if (_fileStorage.FileExists(originalRelative))
                    {
                        var newFile = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                        originalRelative = Path.Combine("patients", "profile-pictures", newFile).Replace("\\", "/");
                    }

                    await _fileStorage.MoveFileAsync(patient.ProfilePhotoUrl, originalRelative);
                    patient.ProfilePhotoUrl = originalRelative;
                }
                else
                {
                    // file missing in recycle-bin; just clear profile path
                    patient.ProfilePhotoUrl = null;
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw; // let caller handle
            }
        }

        public async Task OnHardDeleteAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is not Patient patient) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(patient.ProfilePhotoUrl) && _fileStorage.FileExists(patient.ProfilePhotoUrl))
                {
                    _fileStorage.DeleteFile(patient.ProfilePhotoUrl);
                }

                // delete associated ApplicationUser if exists and not referenced by others
                if (!string.IsNullOrWhiteSpace(patient.UserId))
                {
                    var user = await _userManagement.FindUserAsync(patient.UserId);
                    if (user != null)
                    {
                        // attempt to delete via UserManager
                        var u = await _userManager.FindByIdAsync(user.Id);
                        if (u != null)
                        {
                            await _userManager.DeleteAsync(u);
                        }
                    }
                }
            }
            catch
            {
                throw; // bubble up to caller to abort hard delete
            }
        }
    }
}
