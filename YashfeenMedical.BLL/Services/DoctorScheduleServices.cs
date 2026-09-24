using Mapster;
using MapsterMapper;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Models;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.Infrastructure.Exceptions;

namespace YashfeenMedical.BLL.Services
{
    public class DoctorScheduleServices : TEntityService<DoctorSchedule, int, DoctorScheduleDto, DoctorScheduleCreationDto, DoctorScheduleUpdateDto>, IDoctorScheduleServices
    {
        private readonly IDoctorScheduleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPaginationServices _paginationServices;

        public DoctorScheduleServices(IDoctorScheduleRepository repository, IMapper mapper, IPaginationServices paginationServices)
            : base(repository, mapper, paginationServices)
        {
            _repository = repository;
            _mapper = mapper;
            _paginationServices = paginationServices;
        }

        public async Task<TPaginationQueryModel<DoctorScheduleDto>> GetDoctorSchedule(int doctorId, PaginationQuery paginationQuery)
        {
            var schedule = _repository.GetDoctorScheduleAsync(doctorId);
            var scheduleDtos = schedule.ProjectToType<DoctorScheduleDto>();

            var paggedList = await _paginationServices.GetPaggedList(scheduleDtos, paginationQuery);

            return paggedList;
        }

        public override async Task<DoctorScheduleDto> Add(DoctorScheduleCreationDto creationDto)
        {
            var schedule = _mapper.Map<DoctorSchedule>(creationDto);
            var maxSlots = ValidateSchedule(schedule);

            schedule.MaxAppointmentsPerDay = maxSlots;
            schedule.IsActive = true;

            await _repository.Add(schedule);
            await _repository.SaveChanges();

            return _mapper.Map<DoctorScheduleDto>(schedule);
        }

        public override async Task<DoctorScheduleDto> Update(int id, DoctorScheduleUpdateDto updateDto)
        {
            var schedule = _mapper.Map<DoctorSchedule>(updateDto);
            ValidateSchedule(schedule);

            return await base.Update(id, updateDto);
        }

        private int ValidateSchedule(DoctorSchedule schedule)
        {
            var totalMinutes = (schedule.EndTime.ToTimeSpan() - schedule.StartTime.ToTimeSpan()).TotalMinutes;

            if (totalMinutes <= 0)
                throw new BadRequestException("EndTime must be after StartTime.");

            if (schedule.SlotDurationMinutes <= 0)
                throw new BadRequestException("SlotDurationMinutes must be greater than zero.");

            var maxSlots = (int)Math.Floor(totalMinutes / schedule.SlotDurationMinutes);

            if (schedule.MaxAppointmentsPerDay > maxSlots)
                throw new BadRequestException($"MaxAppointmentsPerDay cannot exceed the number of available slots ({maxSlots}) based on the schedule and slot duration.");

            return maxSlots;
        }
    }
}