using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace YashfeenMedical.BLL.Services
{
    public class AppointmentServices : TEntityService<Appointment, int, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>, IAppointmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaginationServices _paginationServices;

        public AppointmentServices(IAppointmentRepository appointmentRepository, IMapper mapper,
            IPaginationServices paginationServices, IUnitOfWork unitOfWork) : base(appointmentRepository, mapper, paginationServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paginationServices = paginationServices;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> BringAppointmentsTodayAsync(AppointmentQueryModel queryModel, DateOnly date)
        {
            var appointmentsToday = _unitOfWork.Appointments.BringAppointmentsToday(date);
            var filteredAppointments = _unitOfWork.Appointments.GetFilterdAppointments(queryModel, appointmentsToday);
            var appointmentDtos = filteredAppointments.ProjectToType<AppointmentDto>();

            var paggedList = await _paginationServices.GetPaggedList(appointmentDtos, queryModel);

            return paggedList;
        }

        public async Task<TPaginationQueryModel<AppointmentDto>> GetFilterdAppointmentsAsync(AppointmentQueryModel queryModel)
        {
            var appointments = _unitOfWork.Appointments.GetAll();
            var filteredAppointments = _unitOfWork.Appointments.GetFilterdAppointments(queryModel, appointments);
            var appointmentDtos = filteredAppointments.ProjectToType<AppointmentDto>();

            var paggedList = await _paginationServices.GetPaggedList(appointmentDtos, queryModel);

            return paggedList;
        }

        public async override Task<AppointmentDto> Add(AppointmentCreationDto creationDTO)
        {
            await ValidateAppointmentCreationAsync(creationDTO);

            var mappedApointment = _mapper.Map<Appointment>(creationDTO);



            throw new NotImplementedException();
        }

        private async Task ValidatePatientAsync(int patientId)
        {
            var patient = await _unitOfWork.Patients.IsExists(patientId);
            if (!patient)
                throw new NotFoundException("the requested patient dosen't exits");
        }

        private async Task<Doctor> GetDoctorAsync(int doctorId)
        {
            var doctor = await _unitOfWork.Doctors.GetById(doctorId)
                ?? throw new NotFoundException("the requested doctor dosen't exits");

            return doctor;
        }

        private void ValidateAppointmentDate(DateOnly appointmentDate, TimeOnly startTime)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (appointmentDate < today)
                throw new BadRequestException("can't book an appointment in the past");

            if (appointmentDate == today && startTime <= TimeOnly.FromDateTime(DateTime.UtcNow))
                throw new BadRequestException("can't book an appointment for a time that has already passed today");
        }

        private async Task ValidateWithinDoctorScheduleAsync(AppointmentCreationDto creationDTO)
        {
            var dayOfWeek = (ScheduleDayOfWeek)creationDTO.AppointmentDate.DayOfWeek;

            var schedule = await _unitOfWork.DoctorSchedules.GetAll()
                .FirstOrDefaultAsync(s => s.DoctorId == creationDTO.DoctorId
                                        && s.DayOfWeek == dayOfWeek
                                        && s.IsActive);

            if (schedule == null)
                throw new BadRequestException("this doctor dosen't work on this day of the week");

            var endTime = creationDTO.StartTime.AddMinutes(schedule.SlotDurationMinutes);

            if (creationDTO.StartTime < schedule.StartTime || endTime > schedule.EndTime)
                throw new BadRequestException(
                    $"the appointment must be scheduled between {schedule.StartTime:hh\\:mm} and {schedule.EndTime:hh\\:mm}");

            var minutesFromStart = (creationDTO.StartTime - schedule.StartTime).TotalMinutes;
            if (minutesFromStart % schedule.SlotDurationMinutes != 0)
                throw new BadRequestException(
                    $"the appointment time must align with the available booking slots (every {schedule.SlotDurationMinutes} minutes)");
        }

        private async Task ValidateNoDoctorConflictAsync(AppointmentCreationDto creationDTO)
        {
            var hasConflict = await _unitOfWork.Appointments.GetAll()
                .AnyAsync(a => a.DoctorId == creationDTO.DoctorId
                            && a.AppointmentDate == creationDTO.AppointmentDate
                            && a.StartTime == creationDTO.StartTime
                            && !InactiveStatuses.Contains(a.Status));

            if (hasConflict)
                throw new ConflictException("this appointment is already booked with this doctor");
        }

        private async Task ValidateNoPatientConflictAsync(AppointmentCreationDto creationDTO)
        {
            var hasConflict = await _unitOfWork.Appointments.GetAll()
                .AnyAsync(a => a.PatientId == creationDTO.PatientId
                            && a.AppointmentDate == creationDTO.AppointmentDate
                            && a.StartTime == creationDTO.StartTime
                            && !InactiveStatuses.Contains(a.Status));

            if (hasConflict)
                throw new ConflictException("you have another appointment booked at the same time");
        }

        private async Task ValidateMaxAppointmentsPerDayAsync(AppointmentCreationDto creationDTO)
        {
            var dayOfWeek = (ScheduleDayOfWeek)creationDTO.AppointmentDate.DayOfWeek;

            var schedule = await _unitOfWork.DoctorSchedules.GetAll()
                .FirstOrDefaultAsync(s => s.DoctorId == creationDTO.DoctorId && s.DayOfWeek == dayOfWeek);

            var bookedCount = await _unitOfWork.Appointments.GetAll()
                .CountAsync(a => a.DoctorId == creationDTO.DoctorId
                               && a.AppointmentDate == creationDTO.AppointmentDate
                               && !InactiveStatuses.Contains(a.Status));

            if (bookedCount >= schedule.MaxAppointmentsPerDay)
                throw new BadRequestException("you have reached the maximum number of appointments allowed for this doctor on this day");
        }

        private static readonly AppointmentStatus[] InactiveStatuses =
        {
          AppointmentStatus.Cancelled,
          AppointmentStatus.NoShow
        };

        private async Task ValidateAppointmentCreationAsync(AppointmentCreationDto creationDto)
        {

            ValidateAppointmentDate(creationDto.AppointmentDate, creationDto.StartTime);

            var doctor = await GetDoctorAsync(creationDto.DoctorId);

            if (!doctor.IsAvailable)
                throw new BadRequestException("this doctor is not available for new appointments at the moment");

            await ValidatePatientAsync(creationDto.PatientId);

            await ValidateWithinDoctorScheduleAsync(creationDto);

            await ValidateMaxAppointmentsPerDayAsync(creationDto);

            await ValidateNoDoctorConflictAsync(creationDto);

            await ValidateNoPatientConflictAsync(creationDto);
        }

        private Invoice GenerateInvoice(Appointment appointment)
        {
            var invoice = new Invoice()
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{appointment.Id}",
                PaidAmount = appointment.Doctor.ConsultationFee
            };

            return invoice;
        }
    }
}
