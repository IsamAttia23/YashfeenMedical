using System.Threading;
using System.Threading.Tasks;

namespace YashfeenMedical.BLL.RecycleBin
{
    public interface IRecycleBinHandler
    {
        /// <summary>
        /// Entity key, e.g. "patient" or "doctor"
        /// </summary>
        string EntityKey { get; }

        Task OnSoftDeleteAsync(object entity, CancellationToken cancellationToken = default);
        Task OnRestoreAsync(object entity, CancellationToken cancellationToken = default);
        Task OnHardDeleteAsync(object entity, CancellationToken cancellationToken = default);
    }
}
