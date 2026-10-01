using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.Models
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTimeOffset ExpireOn { get; set; }
        public bool IsExpired => DateTimeOffset.Now >= ExpireOn;
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
