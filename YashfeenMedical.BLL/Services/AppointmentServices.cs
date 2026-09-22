using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.BLL.Services
{
    public class AppointmentServices : TEntityService<Appointment, int, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>, IAppointmentServices
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationServices _paginationServices;

        public AppointmentServices(IAppointmentRepository appointmentRepository, IMapper mapper, IPaginationServices paginationServices) : base(appointmentRepository, mapper, paginationServices)
        {
            _appointmentsRepository = appointmentRepository;
            _mapper = mapper;
            _paginationServices = paginationServices;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> BringAppointmentsTodayAsync(AppointmentQueryModel queryModel, DateOnly date)
        {
            var appointmentsToday = _appointmentsRepository.BringAppointmentsToday(date);
            var filteredAppointments = _appointmentsRepository.GetFilterdAppointments(queryModel, appointmentsToday);
            var appointmentDtos = filteredAppointments.ProjectToType<AppointmentDto>();

            var paggedList = await _paginationServices.GetPaggedList(appointmentDtos, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> GetFilterdAppointmentsAsync(AppointmentQueryModel queryModel)
        {
            var appointments = _appointmentsRepository.GetAll();
            var filteredAppointments = _appointmentsRepository.GetFilterdAppointments(queryModel, appointments);
            var appointmentDtos = filteredAppointments.ProjectToType<AppointmentDto>();

            var paggedList = await _paginationServices.GetPaggedList(appointmentDtos, queryModel);

            return paggedList;
        }
    }
}
