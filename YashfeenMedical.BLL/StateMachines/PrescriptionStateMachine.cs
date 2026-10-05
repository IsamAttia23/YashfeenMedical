using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Prescriptions;
using YashfeenMedical.BLL.IStateMachines;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.BLL.StateMachines
{
    public class PrescriptionStateMachine : IPrescriptionStateMachine
    {
        private readonly IPrescriptionRepository _repository;
        private readonly IMapper _mapper;

        public PrescriptionStateMachine(IPrescriptionRepository prescriptionItemRepository, IMapper mapper)
        {
            _repository = prescriptionItemRepository;
            _mapper = mapper;
        }

        public async Task<PrescriptionDto> Cancel(int id)
        {
            var entity = await _repository.GetById(id) ?? throw new NotFoundException("The request entity dosen't exits");

            await EnsureExpiredIfNeeded(entity);

            if (entity.Status != PrescriptionStatus.Active)
                throw new UnprocessableEntityException("Only active prescriptions can be cancelled.");

            entity.Status = PrescriptionStatus.Cancelled;

            await _repository.Update(entity);
            await _repository.SaveChanges();

            return _mapper.Map<PrescriptionDto>(entity);
        }

        public async Task<PrescriptionDto> Dispense(int id)
        {
            var entity = await _repository.GetById(id) ?? throw new NotFoundException("The request entity dosen't exits");

            await EnsureExpiredIfNeeded(entity);

            if (entity.Status != PrescriptionStatus.Active)
                throw new UnprocessableEntityException("Only active prescriptions can be dispensed.");

            entity.Status = PrescriptionStatus.Dispensed;

            await _repository.Update(entity);
            await _repository.SaveChanges();

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
            }
        }
    }
}
