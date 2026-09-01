using YashfeenMedical.DAL.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace YashfeenMedical.DAL.Shared.Entities
{
    // لاحظ: لا يوجد Name هنا لأن Order/OrderItem/Invoice ليس لها Name
    // الكيانات التي لها Name تُضيفها بنفسها
    public class TEntity<TId> : IEntity<TId>
        where TId : struct
    {
        public TId Id { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
