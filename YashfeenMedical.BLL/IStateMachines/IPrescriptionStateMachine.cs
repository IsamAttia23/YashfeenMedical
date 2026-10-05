using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.DTOs.Prescriptions;

namespace YashfeenMedical.BLL.IStateMachines
{
    public interface IPrescriptionStateMachine
    {
        Task<PrescriptionDto> Dispense(int id);
        Task<PrescriptionDto> Cancel(int id);
    }
}
