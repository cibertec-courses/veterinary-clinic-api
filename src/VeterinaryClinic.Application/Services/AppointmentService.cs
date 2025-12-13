using AutoMapper;
using Microsoft.Extensions.Logging;
using VeterinaryClinic.Application.DTOs.Appointment;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Domain.Entities;
using VeterinaryClinic.Domain.Exceptions;
using VeterinaryClinic.Domain.Ports.Out;

namespace VeterinaryClinic.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AppointmentDto> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving appointment with ID {AppointmentId}.", id);

        var appointment = await _unitOfWork.Appointments.GetWithPetAndOwnerAsync(id);
        if (appointment == null)
        {
            _logger.LogWarning("Appointment with ID {AppointmentId} not found.", id);
            throw new NotFoundException("Appointment", id);
        }
        _logger.LogInformation("Appointment on {AppointmentDate} found.", appointment.AppointmentDate);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all appointments.");
        var appointments = await _unitOfWork.Appointments.GetAllAsync();
        _logger.LogInformation("{AppointmentCount} appointments retrieved.", appointments.Count());
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        _logger.LogInformation("Creating a new appointment for Pet ID {PetID} on {AppointmentDate}", dto.PetId, dto.AppointmentDate);
        var pet = await _unitOfWork.Pets.GetWithOwnerAsync(dto.PetId);
        if (pet == null)
        {
            _logger.LogWarning("Pet with ID {PetID} not found. Cannot create appointment.", dto.PetId);
            throw new NotFoundException("Pet", dto.PetId);
        }

        if (dto.AppointmentDate <= DateTime.Now)
        {
            _logger.LogWarning("Attempted to create appointment in the past for Pet ID {PetID} on {AppointmentDate}", dto.PetId, dto.AppointmentDate);
            throw new BusinessRuleException(
                "PastAppointment",
                "Cannot schedule appointments in the past.");
        }

        var appointment = _mapper.Map<Appointment>(dto);

        var createdAppointment = await _unitOfWork.Appointments.CreateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        var appointmentWithDetails = await _unitOfWork.Appointments.GetWithPetAndOwnerAsync(createdAppointment.Id);
        _logger.LogInformation("Appointment for Pet ID {PetID} created successfully with ID {AppointmentID}.", dto.PetId, createdAppointment.Id);
        return _mapper.Map<AppointmentDto>(appointmentWithDetails!);
    }

    public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var appointment = await _unitOfWork.Appointments.GetWithPetAndOwnerAsync(id);
        if (appointment == null)
        {
            throw new NotFoundException("Appointment", id);
        }

        if (dto.AppointmentDate != appointment.AppointmentDate)
        {
            if (dto.AppointmentDate <= DateTime.Now)
            {
                throw new BusinessRuleException(
                    "PastAppointment",
                    "Cannot reschedule to a past date.");
            }
        }

        var validStatuses = new[] { "Scheduled", "Completed", "Cancelled" };
        if (!validStatuses.Contains(dto.Status))
        {
            throw new BusinessRuleException(
                "InvalidStatus",
                $"Status must be one of: {string.Join(", ", validStatuses)}");
        }

        _mapper.Map(dto, appointment);

        var updatedAppointment = await _unitOfWork.Appointments.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AppointmentDto>(updatedAppointment);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete appointment with ID {AppointmentID}.", id);
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            _logger.LogWarning("Appointment with ID {AppointmentID} not found. Cannot delete.", id);
            throw new NotFoundException("Appointment", id);
        }

        var result = await _unitOfWork.Appointments.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Appointment with ID {AppointmentID} deleted successfully.", id);
        return result;
    }

    public async Task<bool> CancelAsync(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            throw new NotFoundException("Appointment", id);
        }

        if (!appointment.CanBeCancelled())
        {
            throw new BusinessRuleException(
                "CannotCancel",
                "Only future scheduled appointments can be cancelled.");
        }

        appointment.Status = "Cancelled";

        await _unitOfWork.Appointments.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<AppointmentDto>> GetByPetIdAsync(int petId)
    {
        var petExists = await _unitOfWork.Pets.ExistsAsync(petId);
        if (!petExists)
        {
            throw new NotFoundException("Pet", petId);
        }

        var appointments = await _unitOfWork.Appointments.GetByPetIdAsync(petId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new BusinessRuleException(
                "InvalidDateRange",
                "Start date must be before end date.");
        }

        var appointments = await _unitOfWork.Appointments.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetByStatusAsync(string status)
    {
        var validStatuses = new[] { "Scheduled", "Completed", "Cancelled" };
        if (!validStatuses.Contains(status))
        {
            throw new BusinessRuleException(
                "InvalidStatus",
                $"Status must be one of: {string.Join(", ", validStatuses)}");
        }

        var appointments = await _unitOfWork.Appointments.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
}