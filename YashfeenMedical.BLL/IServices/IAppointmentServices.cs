using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Appointments;

namespace YashfeenMedical.BLL.IServices
{
    public interface IAppointmentServices : IEntityServices<int,AppointmentDto,AppointmentCreationDto,AppointmentUpdateDto>
    {
    }
}
