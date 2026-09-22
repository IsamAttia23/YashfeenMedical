using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.BLL.IStateMachines;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.BLL.StateMachines
{
    public class AppointmentStateMachine : IAppointmentStateMachine
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentStateMachine(IAppointmentRepository appointmentRepository,IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public Task<AppointmentDto> AppointmentSetAppointmentAsNoShowAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, string cancelReason)
        {
            throw new NotImplementedException();
        }

        public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetById(appointmentId)
                ?? throw new NotFoundException("the requested appointment dosen't exits");

            if (appointment.Status != AppointmentStatus.Scheduled)
                throw new UnprocessableEntityException("you can only confirm the Scheduled appointment");

            appointment.ConfirmedAt = DateTimeOffset.Now;

            await _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChanges();

            var result = _mapper.Map<AppointmentDto>(appointment);

            return result;
        }

        public Task<AppointmentDto> StartAppointmentAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }
    }
}
