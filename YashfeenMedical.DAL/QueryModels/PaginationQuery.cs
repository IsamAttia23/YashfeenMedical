using YashfeenMedical.DAL.Enums;

namespace YashfeenMedical.DAL.QueryModels;

public class PaginationQuery
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
    public string? SearchTerm { get; set; }
}
