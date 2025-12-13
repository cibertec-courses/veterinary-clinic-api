using AutoMapper;
using VeterinaryClinic.Application.DTOs.Pet;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Domain.Entities;
using VeterinaryClinic.Domain.Exceptions;
using VeterinaryClinic.Domain.Ports.Out;

namespace VeterinaryClinic.Application.Services;

public class PetService : IPetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PetService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PetDto> GetByIdAsync(int id)
    {
        var pet = await _unitOfWork.Pets.GetWithOwnerAsync(id);
        if (pet == null)
        {
            throw new NotFoundException("Pet", id);
        }
        return _mapper.Map<PetDto>(pet);
    }

    public async Task<IEnumerable<PetDto>> GetAllAsync()
    {
        var pets = await _unitOfWork.Pets.GetAllAsync();
        return _mapper.Map<IEnumerable<PetDto>>(pets);
    }

    public async Task<PetDto> CreateAsync(CreatePetDto dto)
    {
        var ownerExists = await _unitOfWork.Owners.ExistsAsync(dto.OwnerId);
        if (!ownerExists)
        {
            throw new NotFoundException("Owner", dto.OwnerId);
        }

        if (dto.BirthDate > DateTime.Today)
        {
            throw new BusinessRuleException(
                "InvalidBirthDate", 
                "Birth date cannot be in the future.");
        }

        var pet = _mapper.Map<Pet>(dto);

        var createdPet = await _unitOfWork.Pets.CreateAsync(pet);
        await _unitOfWork.SaveChangesAsync();

        var petWithOwner = await _unitOfWork.Pets.GetWithOwnerAsync(createdPet.Id);
        return _mapper.Map<PetDto>(petWithOwner!);
    }

    public async Task<PetDto> UpdateAsync(int id, UpdatePetDto dto)
    {
        var pet = await _unitOfWork.Pets.GetWithOwnerAsync(id);
        if (pet == null)
        {
            throw new NotFoundException("Pet", id);
        }

        if (dto.BirthDate > DateTime.Today)
        {
            throw new BusinessRuleException(
                "InvalidBirthDate", 
                "Birth date cannot be in the future.");
        }

        _mapper.Map(dto, pet);

        var updatedPet = await _unitOfWork.Pets.UpdateAsync(pet);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PetDto>(updatedPet);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pet = await _unitOfWork.Pets.GetByIdAsync(id);
        if (pet == null)
        {
            throw new NotFoundException("Pet", id);
        }

        var appointments = await _unitOfWork.Appointments.GetByPetIdAsync(id);
        var pendingAppointments = appointments.Where(a => a.Status == "Scheduled").ToList();
        
        if (pendingAppointments.Any())
        {
            throw new BusinessRuleException(
                "PetHasPendingAppointments", 
                $"Cannot delete pet with ID {id} because they have {pendingAppointments.Count} pending appointments.");
        }

        var result = await _unitOfWork.Pets.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<PetDto>> GetByOwnerIdAsync(int ownerId)
    {
        var ownerExists = await _unitOfWork.Owners.ExistsAsync(ownerId);
        if (!ownerExists)
        {
            throw new NotFoundException("Owner", ownerId);
        }

        var pets = await _unitOfWork.Pets.GetByOwnerIdAsync(ownerId);
        return _mapper.Map<IEnumerable<PetDto>>(pets);
    }

    public async Task<IEnumerable<PetDto>> GetBySpeciesAsync(string species)
    {
        var pets = await _unitOfWork.Pets.GetBySpeciesAsync(species);
        return _mapper.Map<IEnumerable<PetDto>>(pets);
    }
}