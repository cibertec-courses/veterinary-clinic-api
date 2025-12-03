using AutoMapper;
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

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetWithPetAndOwnerAsync(id);
        return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _unitOfWork.Appointments.GetAllAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        var pet = await _unitOfWork.Pets.GetWithOwnerAsync(dto.PetId);
        if (pet == null)
        {
            throw new NotFoundException("Pet", dto.PetId);
        }

        if (dto.AppointmentDate <= DateTime.Now)
        {
            throw new BusinessRuleException(
                "PastAppointment", 
                "Cannot schedule appointments in the past.");
        }

        var appointment = _mapper.Map<Appointment>(dto);

        var createdAppointment = await _unitOfWork.Appointments.CreateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        var appointmentWithDetails = await _unitOfWork.Appointments.GetWithPetAndOwnerAsync(createdAppointment.Id);
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
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
        {
            throw new NotFoundException("Appointment", id);
        }

        var result = await _unitOfWork.Appointments.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

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