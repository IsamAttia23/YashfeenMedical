using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YashfeenMedical.DAL;
using YashfeenMedical.DAL.Shared.Entities;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.BLL.RecycleBin;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.BLL.IServices;

namespace YashfeenMedical.BLL.RecycleBin
{
    public class RecycleBinService
    {
        private readonly ApplicationDbContext _db;
        private readonly IServiceProvider _sp;

        private readonly Dictionary<string, Type> _supported = new()
        {
            { "patient", typeof(Patient) },
            { "doctor", typeof(Doctor) },
            { "appointment", typeof(Appointment) },
            { "medicalrecord", typeof(MedicalRecord) },
            { "prescription", typeof(Prescription) },
            { "invoice", typeof(Invoice) },
            {"specialty", typeof(Specialty) }
        };

        private readonly IPaginationServices _paginationServices;

        public RecycleBinService(ApplicationDbContext db, IServiceProvider sp, IPaginationServices paginationServices)
        {
            _db = db;
            _sp = sp;
            _paginationServices = paginationServices;
        }

        public bool IsSupported(string key) => _supported.ContainsKey(key.ToLowerInvariant());

        private Type Resolve(string key)
        {
            if (!_supported.TryGetValue(key.ToLowerInvariant(), out var t))
                throw new ArgumentException("Unsupported entity type", nameof(key));
            return t;
        }

        public async Task<TPaginationQueryModel<RecycleBinItemDto>> List(string entityType, PaginationQuery query, CancellationToken ct = default)
        {
            if (!IsSupported(entityType)) throw new ArgumentException("Unsupported entity type", nameof(entityType));

            var t = Resolve(entityType);
            var method = typeof(DbContext).GetMethod("Set", Type.EmptyTypes)!.MakeGenericMethod(t);
            var set = (IQueryable)method.Invoke(_db, null)!;

            // deleted items: DeletedOn != null
            var deleted = set.Cast<IEntity<int>>().Where(x => x.DeletedOn != null);

            var projected = deleted.Select(x => new RecycleBinItemDto
            {
                EntityType = entityType.ToLowerInvariant(),
                EntityId = Convert.ToInt32(x.Id),
                DeletedOn = x.DeletedOn!.Value
            }).AsQueryable();
            var pagged = await _paginationServices.GetPaggedList(projected, query);
            return pagged;
        }

        public async Task Restore(string entityType, int id, CancellationToken ct = default)
        {
            if (!IsSupported(entityType)) throw new ArgumentException("Unsupported entity type", nameof(entityType));

            var t = Resolve(entityType);
            var method = typeof(DbContext).GetMethod("Set", Type.EmptyTypes)!.MakeGenericMethod(t);
            var set = (IQueryable)method.Invoke(_db, null)!;

            var entity = await set.Cast<IEntity<int>>().FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
            if (entity == null) throw new KeyNotFoundException("Entity not found");
            if (entity.DeletedOn == null) throw new InvalidOperationException("Entity is not deleted");

            // call handler if any
            var handler = GetHandler(entityType);
            if (handler != null)
            {
                await handler.OnRestoreAsync(entity, ct);
            }

            entity.DeletedOn = null;
            await _db.SaveChangesAsync(ct);
        }

        public async Task HardDelete(string entityType, int id, CancellationToken ct = default)
        {
            if (!IsSupported(entityType)) throw new ArgumentException("Unsupported entity type", nameof(entityType));

            var t = Resolve(entityType);
            var method = typeof(DbContext).GetMethod("Set", Type.EmptyTypes)!.MakeGenericMethod(t);
            var set = (IQueryable)method.Invoke(_db, null)!;

            var entity = await set.Cast<IEntity<int>>().FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
            if (entity == null) throw new KeyNotFoundException("Entity not found");

            var handler = GetHandler(entityType);
            if (handler != null)
            {
                await handler.OnHardDeleteAsync(entity, ct);
            }

            _db.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }

        private IRecycleBinHandler? GetHandler(string entityType)
        {
            // resolve handler by matching EntityKey
            var handlers = _sp.GetService(typeof(IEnumerable<IRecycleBinHandler>)) as IEnumerable<IRecycleBinHandler>;
            return handlers?.FirstOrDefault(h => string.Equals(h.EntityKey, entityType, StringComparison.OrdinalIgnoreCase));
        }
    }
}
