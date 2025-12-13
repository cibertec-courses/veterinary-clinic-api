
using AutoMapper;
using Microsoft.Extensions.Logging;
using VeterinaryClinic.Application.DTOs.Owner;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Domain.Entities;
using VeterinaryClinic.Domain.Exceptions;
using VeterinaryClinic.Domain.Ports.Out;

namespace VeterinaryClinic.Application.Services
{
    public class OwnerService: IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerService> _logger;

        public OwnerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OwnerDto> CreateAsync(CreateOwnerDto ownerDto)
        {
            _logger.LogInformation("Creating a new owner with email {OwnerEmail}", ownerDto.Email);
            var existingOwner = await _unitOfWork.Owners.GetByEmailAsync(ownerDto.Email);
            if (existingOwner != null)
            {
                _logger.LogWarning("Owner with email {OwnerEmail} already exists. Cannot create duplicate.", ownerDto.Email);
                throw new DuplicateEntityException("Owner","Email", ownerDto.Email);
            }
            var owner = _mapper.Map<Owner>(ownerDto);
            var createdOwner = await _unitOfWork.Owners.CreateAsync(owner);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Owner with email {OwnerEmail} created successfully with ID {OwnerID}.", ownerDto.Email, createdOwner.Id);
            return _mapper.Map<OwnerDto>(createdOwner);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            
            var owner = await _unitOfWork.Owners.GetWithPetsAsync(id);
            if (owner == null)
            {
                throw new NotFoundException("Owner", id);
            }

            if (owner.Pets.Any())
            {
                throw new BusinessRuleException(
                    "Owner has pets",
                    $"Cannot delete owner with ID {id} because they have pets {owner.Pets.Count} registered. "
                );
            }

            var result = await _unitOfWork.Owners.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return result;
            
        }

        public async Task<IEnumerable<OwnerDto?>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all owners.");
             var owners = await _unitOfWork.Owners.GetAllAsync();
             
            _logger.LogInformation("{OwnerCount} owners retrieved.", owners.Count());
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }

        public async Task<OwnerDto?> GetByEmailAsync(string email)
        {
            var owner = await _unitOfWork.Owners.GetByEmailAsync(email);
            return owner !=null ? null : _mapper.Map<OwnerDto>(owner);
        }

        public async Task<OwnerDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving owner with ID {OwnerId}.", id);
            var owner = await _unitOfWork.Owners.GetByIdAsync(id);
            if ( owner == null)
            {
                _logger.LogWarning("Owner with ID {OwnerId} not found.", id);
                throw new NotFoundException("Owner", id);
            }
            _logger.LogInformation("Owner {OwnerName} found.", owner.FirstName);
            return _mapper.Map<OwnerDto>(owner);
        }

        public async Task<IEnumerable<OwnerDto>> SearchByNameAsync(string name)
        {
            var owner = await _unitOfWork.Owners.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<OwnerDto>>(owner);
        }

        public async Task<OwnerDto> UpdateAsync(int id, UpdateOwnerDto ownerDto)
        {
            var owner = await _unitOfWork.Owners.GetByIdAsync(id);
            if (owner == null)
            {
                throw new NotFoundException("Owner", id);
            }

            var existingOwner = await _unitOfWork.Owners.GetByEmailAsync(ownerDto.Email);
            if (existingOwner != null && existingOwner.Id != id)
            {
                throw new DuplicateEntityException("Owner", "Email", ownerDto.Email);
            }

            _mapper.Map(ownerDto, owner);
            var updatedOwner = await _unitOfWork.Owners.UpdateAsync(owner);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OwnerDto>(updatedOwner);
        }
    }
}