using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.Infrastructure.FileStorage;
using YashfeenMedical.Infrastructure.UsersManagment;

namespace YashfeenMedical.BLL.RecycleBin.Handlers
{
    public class DoctorRecycleBinHandler : IRecycleBinHandler
    {
        public string EntityKey => "doctor";

        private readonly IFileStorageService _fileStorage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagmentServices _userManagement;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorRecycleBinHandler(IFileStorageService fileStorage, IUnitOfWork unitOfWork,
            IUserManagmentServices userManagement, UserManager<ApplicationUser> userManager)
        {
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
            _userManagement = userManagement;
            _userManager = userManager;
        }

        public async Task OnSoftDeleteAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is not Doctor doctor) return;

            if (string.IsNullOrWhiteSpace(doctor.ProfilePhotoUrl)) return;

            var fileName = Path.GetFileName(doctor.ProfilePhotoUrl);
            var destRelative = Path.Combine("recycle-bin", "doctors", doctor.Id.ToString(), fileName).Replace("\\", "/");

            try
            {
                await _fileStorage.MoveFileAsync(doctor.ProfilePhotoUrl, destRelative);
                doctor.ProfilePhotoUrl = destRelative;
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                // swallow to avoid breaking delete
            }
        }

        public async Task OnRestoreAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is not Doctor doctor) return;

            if (string.IsNullOrWhiteSpace(doctor.ProfilePhotoUrl)) return;

            var fileName = Path.GetFileName(doctor.ProfilePhotoUrl);
            var originalRelative = Path.Combine("doctors", "profile-pictures", fileName).Replace("\\", "/");

            try
            {
                if (_fileStorage.FileExists(doctor.ProfilePhotoUrl))
                {
                    if (_fileStorage.FileExists(originalRelative))
                    {
                        var newFile = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                        originalRelative = Path.Combine("doctors", "profile-pictures", newFile).Replace("\\", "/");
                    }

                    await _fileStorage.MoveFileAsync(doctor.ProfilePhotoUrl, originalRelative);
                    doctor.ProfilePhotoUrl = originalRelative;
                }
                else
                {
                    doctor.ProfilePhotoUrl = null;
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task OnHardDeleteAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is not Doctor doctor) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(doctor.ProfilePhotoUrl) && _fileStorage.FileExists(doctor.ProfilePhotoUrl))
                {
                    _fileStorage.DeleteFile(doctor.ProfilePhotoUrl);
                }

                if (!string.IsNullOrWhiteSpace(doctor.UserId))
                {
                    var user = await _userManagement.FindUserAsync(doctor.UserId);
                    if (user != null)
                    {
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
                throw;
            }
        }
    }
}
