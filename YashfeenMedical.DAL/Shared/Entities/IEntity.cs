using System.ComponentModel.DataAnnotations;

namespace YashfeenMedical.DAL.Shared.Entities
{
    public interface IEntity<TId> : TIdType<TId>
        where TId : struct
    {
        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
