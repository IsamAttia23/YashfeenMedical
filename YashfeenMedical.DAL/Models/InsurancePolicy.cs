using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Models;

public class InsurancePolicy : TEntity<int>
{
    public int Id { get; set; }

    public int PatientId { get; set; } // UNIQUE — تأمين واحد فعّال لكل مريض
    public Patient Patient { get; set; } = null!;

    public string InsuranceCompany { get; set; } = string.Empty;

    public string PolicyNumber { get; set; } = string.Empty; // فريد

    public decimal CoveragePercent { get; set; } // 0-100

    public decimal MaxAnnualCoverage { get; set; }

    public decimal UsedCoverage { get; set; } = 0;

    public InsuranceStatus Status { get; set; } = InsuranceStatus.Active;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
