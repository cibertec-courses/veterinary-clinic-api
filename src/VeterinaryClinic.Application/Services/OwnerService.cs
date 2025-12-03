
using AutoMapper;
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

        public OwnerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OwnerDto> CreateAsync(CreateOwnerDto ownerDto)
        {
            var existingOwner = await _unitOfWork.Owners.GetByEmailAsync(ownerDto.Email);
            if (existingOwner != null)
            {
                throw new DuplicateEntityException("Owner","Email", ownerDto.Email);
            }
            var owner = _mapper.Map<Owner>(ownerDto);
            var createdOwner = await _unitOfWork.Owners.CreateAsync(owner);
            await _unitOfWork.SaveChangesAsync();
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
             var owners = await _unitOfWork.Owners.GetAllAsync();
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }

        public async Task<OwnerDto?> GetByEmailAsync(string email)
        {
            var owner = await _unitOfWork.Owners.GetByEmailAsync(email);
            return owner !=null ? null : _mapper.Map<OwnerDto>(owner);
        }

        public async Task<OwnerDto?> GetByIdAsync(int id)
        {
          var owner = await _unitOfWork.Owners.GetByIdAsync(id);
            return owner !=null ? null : _mapper.Map<OwnerDto>(owner);
        }

        public async Task<IEnumerable<OwnerDto>> SearchByNameAsync(string name)
        {
            var owner = await _unitOfWork.Owners.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<OwnerDto>>(owner);
        }

        public async Task<OwnerDto> UpdateAsync(int id, UpdateOwnerDto ownerDto)
        {
            throw new NotImplementedException();
        }
    }
}