using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.Services
{
    public class DoctorServices : TEntityService<Doctor, int, DoctorDto, DoctorCreationDto, DoctorUpdateDto>, IDoctorServices
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public DoctorServices(IDoctorRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TPaginationQueryModel<DoctorDto>> GetFilteredDoctorsWithPaginationAsync(DoctorQueryModel queryModel)
        {
            var doctors = await _repository.GetFilteredDoctorsAsync(queryModel);
            var paginatedDoctors = await _repository.GetPaggedList(doctors, queryModel);

            var result = _mapper.Map<TPaginationQueryModel<DoctorDto>>(paginatedDoctors);

            return result;
        }
    }
}
