using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Invoices;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.BLL.IStateMachines;
using YashfeenMedical.DAL.Enums;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.BLL.StateMachines
{
    public class AppointmentStateMachine : IAppointmentStateMachine
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentStateMachine(IAppointmentRepository appointmentRepository, IMapper mapper
            , IUnitOfWork unitOfWork)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> SetAppointmentAsNoShowAsync(int appointmentId, NoShowAppointmentDto noShowAppointment)
        {
            var appointment = await GetAppointmentAsync(appointmentId);

            if (appointment == null)
                throw new NotFoundException("Appointment not found.");

            if (appointment.Status != AppointmentStatus.Scheduled &&
                appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    "Only scheduled or confirmed appointments can be marked as no-show.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                appointment.Status = AppointmentStatus.NoShow;

                if (appointment.Invoice != null)
                {
                    if (noShowAppointment.ChargePatient)
                    {
                        appointment.Invoice.PaymentStatus = PaymentStatus.Pending;
                    }
                    else
                    {
                        appointment.Invoice.PaymentStatus = PaymentStatus.Cancelled;
                        appointment.Invoice.CancelledAt = DateTimeOffset.Now;
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<AppointmentDto>(appointment);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, CancelAppointmentDto cancellationReason)
        {
            if (string.IsNullOrWhiteSpace(cancellationReason.CancellationReason))
                throw new BadRequestException("Cancellation reason is required.");

            var appointment = await GetAppointmentAsync(appointmentId);

            ValidateAppointmentStatusForCancellation(appointment);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.CancellationReason = cancellationReason.CancellationReason.Trim();
                appointment.CancelledAt = DateTimeOffset.Now;

                if (appointment.Invoice != null)
                {
                    appointment.Invoice.PaymentStatus = PaymentStatus.Cancelled;
                    appointment.Invoice.CancelledAt = DateTimeOffset.Now;
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<AppointmentDto>(appointment);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await GetAppointmentAsync(appointmentId);

            if (appointment.Status != AppointmentStatus.Scheduled)
                throw new UnprocessableEntityException("you can only confirm the Scheduled appointments");

            appointment.ConfirmedAt = DateTimeOffset.Now;
            appointment.Status = AppointmentStatus.Confirmed;

            await _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChanges();

            return await SaveChengesAsync(appointment);
        }

        public async Task<AppointmentDto> StartAppointmentAsync(int appointmentId)
        {
            var appointment = await GetAppointmentAsync(appointmentId);

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new UnprocessableEntityException("you can only start the  Confirmed appointments");

            appointment.StartedAt = DateTimeOffset.Now;
            appointment.Status = AppointmentStatus.InProgress;

            return await SaveChengesAsync(appointment);
        }

        public async Task<(AppointmentDto, MedicalRecordDto)> CompleteAppointmentAsync(int appointmentId, MedicalRecordCreationDto medicalRecord)
        {
            var appointment = await GetAppointmentAsync(appointmentId);

            if (appointment.Status != AppointmentStatus.InProgress)
                throw new UnprocessableEntityException("you can only complete the InProgress appointments");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                appointment.CompletedAt = DateTimeOffset.Now;
                appointment.Status = AppointmentStatus.Completed;

                await _appointmentRepository.Update(appointment);
                var medicalRecordDto = await CreateInitialMedicalRecord(appointment, medicalRecord);

                await RecalculateInvoiceAsync(appointment, medicalRecord.InvoiceItems);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

                return (appointmentDto, medicalRecordDto);
            }
            catch (AppException)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InternalServerErorrException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        private async Task<Appointment> GetAppointmentAsync(int appointmentId)
        {
            return await _appointmentRepository.GetById(appointmentId)
                ?? throw new NotFoundException("the requested appointment dosen't exits");
        }
        private async Task<AppointmentDto> SaveChengesAsync(Appointment appointment)
        {
            await _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChanges();

            var result = _mapper.Map<AppointmentDto>(appointment);

            return result; ;
        }
        private async Task<MedicalRecordDto> CreateInitialMedicalRecord(Appointment appointment, MedicalRecordCreationDto creationDto)
        {
            creationDto.AppointmentId = appointment.Id;
            var medicalRecord = _mapper.Map<MedicalRecord>(creationDto);
            await _unitOfWork.MedicalRecords.Add(medicalRecord);

            var result = _mapper.Map<MedicalRecordDto>(medicalRecord);
            return result;
        }
        private void ValidateAppointmentStatusForCancellation(Appointment appointment)
        {
            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new BadRequestException("Appointment is already cancelled.");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new BadRequestException("Completed appointment cannot be cancelled.");

            if (appointment.Status != AppointmentStatus.Scheduled && appointment.Status != AppointmentStatus.Confirmed)
                throw new UnprocessableEntityException("you can only cancel the Scheduled or Confirmed appointments");
        }
        private async Task RecalculateInvoiceAsync(Appointment appointment, IList<InvoiceItemCreationDto>? invoiceItems)
        {
            var invoice = await _unitOfWork.Invoices.GetAll()
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.AppointmentId == appointment.Id)
                ?? throw new NotFoundException("Invoice not found for this appointment");

            if (invoiceItems != null && invoiceItems.Any())
            {
                foreach (var item in invoiceItems)
                {
                    var discountMultiplier = 1 - ((item.DiscountPercent ?? 0) / 100);

                    var mappedItem = _mapper.Map<InvoiceItem>(item);

                    mappedItem.Total = item.Quantity * item.UnitPrice * discountMultiplier;

                    invoice.Items.Add(mappedItem);
                }
            }

            invoice.SubTotal = invoice.Items.Sum(i => i.Total);
            invoice.TotalAmount = invoice.SubTotal
                                 - invoice.DiscountAmount
                                 - invoice.InsuranceCoverage
                                 + invoice.TaxAmount;

            await _unitOfWork.Invoices.Update(invoice);
        }
    }
}
