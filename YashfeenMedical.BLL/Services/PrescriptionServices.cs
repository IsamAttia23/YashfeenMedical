using MapsterMapper;
using YashfeenMedical.BLL.DTOs.Prescriptions;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.BLL.Services
{
    public class PrescriptionServices : TEntityService<Prescription, int, PrescriptionDto, PrescriptionCreationDto, PrescriptionUpdateDto>, IPrescriptionServices
    {
        private readonly IPrescriptionRepository _repository;
        private readonly IMapper _mapper;

        public PrescriptionServices(IPrescriptionRepository repository, IMapper mapper, IPaginationServices paginationServices) : base(repository, mapper, paginationServices)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<PrescriptionDto> Add(PrescriptionCreationDto creationDTO)
        {
            var entity = _mapper.Map<Prescription>(creationDTO);

            entity.IssuedAt = DateTimeOffset.Now;
            entity.Status = PrescriptionStatus.Active;

            if (!creationDTO.ExpiresAt.HasValue)
            {
                var expires = DateTimeOffset.UtcNow.AddDays(1);
                entity.ExpiresAt = DateOnly.FromDateTime(expires.Date);
            }
            else
            {
                entity.ExpiresAt = creationDTO.ExpiresAt.Value;
            }

            // Ensure items list is not null
            entity.Items = entity.Items ?? new List<DAL.Models.PrescriptionItem>();

            await _repository.Add(entity);
            await _repository.SaveChanges();

            var result = _mapper.Map<PrescriptionDto>(entity);
            return result;
        }

        public override async Task<PrescriptionDto?> Details(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            await EnsureExpiredIfNeeded(entity);

            return _mapper.Map<PrescriptionDto>(entity);
        }

        private async Task EnsureExpiredIfNeeded(Prescription entity)
        {
            if (entity.Status == PrescriptionStatus.Expired) return;

            var expiryDateTime = entity.IssuedAt.AddDays(1);
            if (DateTimeOffset.UtcNow > expiryDateTime)
            {
                entity.Status = PrescriptionStatus.Expired;
                await _repository.Update(entity);
                await _repository.SaveChanges();
            }
        }
    }
}
