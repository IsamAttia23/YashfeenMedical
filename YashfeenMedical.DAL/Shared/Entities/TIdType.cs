using System.ComponentModel.DataAnnotations;

namespace YashfeenMedical.DAL.Shared.Entities
{
    public interface TIdType<TId>
        where TId : struct
    {
        [Required]
        public TId Id { get; set; }
    }
}
