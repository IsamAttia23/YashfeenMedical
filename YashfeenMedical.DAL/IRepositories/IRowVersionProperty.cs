using System;
using System.Collections.Generic;
using System.Text;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IRowVersionProperty
    {
        public byte[] RowVersion { get; set; }
    }
}
