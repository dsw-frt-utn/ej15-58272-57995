using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Domain
{
    public interface IPersistence
    {
        Task<IEnumerable<Doctor>> GetActiveDoctorsAsync();
        Task<Doctor?> GetActiveDoctorByIdAsync(Guid id);
        Task AddDoctorAsync(Doctor doctor);
        Task<Speciality?> GetSpecialityByIdAsync(Guid id);
    }
}