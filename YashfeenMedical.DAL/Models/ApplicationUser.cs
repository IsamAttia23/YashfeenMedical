using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public DateTimeOffset LastLogin { get; set; }
        public IList<RefreshToken>? RefreshTokens { get; set; }
        public DateTimeOffset RefreshTokenExpiresAt { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<MedicalFile> UploadedFiles { get; set; } = new List<MedicalFile>();
    }
}
