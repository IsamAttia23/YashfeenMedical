using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Specialties;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.Services
{
    public class SpecialtyServices : TEntityService<Specialty, int, SpecialtyDto, SpecialtyCreationDto, SpecialtyUpdateDto>, ISpecialtyServices
    {
        private readonly ISpecialtyRepository _repository;
        private readonly IMapper _mapper;

        public SpecialtyServices(ISpecialtyRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


    }
}
