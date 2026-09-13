using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Specialties;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.BLL.Services
{
    public class SpecialtyServices : TEntityService<Specialty, int, SpecialtyDto, SpecialtyCreationDto, SpecialtyUpdateDto>, ISpecialtyServices
    {
        private readonly ISpecialtyRepository _repository;

        private readonly IPaginationServices _paginationServices;
        private readonly IMapper _mapper;

        public SpecialtyServices(ISpecialtyRepository repository, IMapper mapper,
            IPaginationServices paginationServices) : base(repository, mapper, paginationServices)
        {
            _repository = repository;
            _mapper = mapper;
            _paginationServices = paginationServices;
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            var specialty = await _repository.GetById(id)
                ?? throw new NotFoundException($"Specialty with id {id} not found");
            return true;
        }
    }
}
