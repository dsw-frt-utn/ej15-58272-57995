using System;
using System.Threading.Tasks;
using Dsw2026Ej15.Api.Dtos; // Importa los DTOs de entrada/salida
using Dsw2026Ej15.Domain;   // Importa la interfaz IPersistence y las entidades
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [cite_start]
    [Route("api/doctors")] // Ruta base general para el controlador [cite: 56, 74]
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        // --- SECCIÓN CONSTRUCTOR (Inyección de dependencias) ---
        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        // --- BLOQUE 1: INSERTAR UN NUEVO MÉDICO ---
        [cite_start]
        [HttpPost] // [cite: 55]
        public async Task<IActionResult> Create([FromBody] DoctorCreateDto dto)
        {
            [cite_start]// Validaciones obligatorias [cite: 64]
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("Name es requerido."); // [cite: 65, 97]

            if (string.IsNullOrWhiteSpace(dto.LicenseNumber))
                throw new ValidationException("License Number es requerido."); // [cite: 66, 97]

            var speciality = await _persistence.GetSpecialityByIdAsync(dto.SpecialityId);
            if (speciality == null)
                throw new ValidationException("La especialidad provista no existe."); // [cite: 67, 97]

            [cite_start]// Creación de la entidad (IsActive nace en true por defecto) [cite: 68]
            var doctor = new Doctor
            {
                Name = dto.Name,
                LicenseNumber = dto.LicenseNumber,
                Speciality = speciality
            };

            await _persistence.AddDoctorAsync(doctor);
            return StatusCode(201, doctor); // 201 Created [cite: 69]
        }

        // --- BLOQUE 2: OBTENER TODOS LOS MÉDICOS ACTIVOS ---
        [cite_start]
        [HttpGet] // [cite: 73]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _persistence.GetActiveDoctorsAsync();
            return Ok(doctors); // 200 OK con la colección [cite: 76]
        }

        // --- BLOQUE 3: OBTENER UN MÉDICO ACTIVO POR ID ---
        [cite_start]
        [HttpGet("{id:guid}")] // Ruta: api/doctors/{id} [cite: 80]
        public async Task<IActionResult> GetById(Guid id)
        {
            var doctor = await _persistence.GetActiveDoctorByIdAsync(id);

            [cite_start]// Validación: Debe existir y estar activo [cite: 82]
            if (doctor == null) return NotFound(); // 404 Not Found [cite: 87]

            [cite_start]// Mapeo plano para mostrar solo lo solicitado [cite: 83]
            var response = new DoctorResponseDto(doctor.Name, doctor.LicenseNumber, doctor.Speciality.Name); // [cite: 84, 85, 86]
            return Ok(response); // 200 OK [cite: 83]
        }

        // --- BLOQUE 4: ESTABLECER COMO INACTIVO (Baja lógica) ---
        [cite_start]
        [HttpDelete("{id:guid}")] // Ruta: api/doctors/{id} [cite: 91, 92]
        public async Task<IActionResult> Delete(Guid id)
        {
            var doctor = await _persistence.GetActiveDoctorByIdAsync(id);

            [cite_start]// Validación: Debe existir y estar activo [cite: 94]
            if (doctor == null) return NotFound(); // 404 Not Found [cite: 96]

            doctor.IsActive = false; // Desactivación (Baja lógica) [cite: 93]
            return NoContent(); // 204 No Content [cite: 95]
        }
    }
}