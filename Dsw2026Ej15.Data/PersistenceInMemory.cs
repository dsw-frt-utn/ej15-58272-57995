using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
      
        private readonly ConcurrentBag<Doctor> _doctors = new();
        private readonly ConcurrentBag<Speciality> _specialities = new();

        public PersistenceInMemory()
        {
         
            LoadSpecialities().GetAwaiter().GetResult();
        }

        
        private async Task LoadSpecialities()
        {
            try
            {
              
                var path = Path.Combine(AppContext.BaseDirectory, "specialities.json");
                if (File.Exists(path))
                {
                    var json = await File.ReadAllTextAsync(path);
                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    var list = JsonSerializer.Deserialize<List<Speciality>>(json, options);
                    if (list != null)
                    {
                        foreach (var spec in list) _specialities.Add(spec);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
        {
            return await Task.FromResult(_doctors.Where(d => d.IsActive));
        }

        public async Task<Doctor?> GetActiveDoctorByIdAsync(Guid id)
        {
            return await Task.FromResult(_doctors.FirstOrDefault(d => d.Id == id && d.IsActive));
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            _doctors.Add(doctor);
            await Task.CompletedTask;
        }

        public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
        {
            return await Task.FromResult(_specialities.FirstOrDefault(s => s.Id == id));
        }
    }
}