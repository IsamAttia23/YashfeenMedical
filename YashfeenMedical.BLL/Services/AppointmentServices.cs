using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;

namespace YashfeenMedical.BLL.Services
{
    public class AppointmentServices : TEntityService<Appointment, int, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>, IAppointmentServices
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationServices _paginationServices;

        public AppointmentServices(IAppointmentRepository appointmentRepository, IMapper mapper, IPaginationServices paginationServices) : base(repository, mapper, paginationServices)
        {
            _appointmentsRepository = appointmentRepository;
            _mapper = mapper;
            _paginationServices = paginationServices;
        }
    }
}
