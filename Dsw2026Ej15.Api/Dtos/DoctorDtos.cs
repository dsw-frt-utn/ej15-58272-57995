using System;

namespace Dsw2026Ej15.Api.Dtos
{
	
	public record DoctorCreateDto(string Name, string LicenseNumber, Guid SpecialityId);

	
	public record DoctorResponseDto(string Name, string LicenseNumber, string SpecialityName);
}