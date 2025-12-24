
using Grpc.Core;
using VeterinaryClinic.Application.DTOs.Pet;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Grpc.Protos;

namespace VeterinaryClinic.Grpc.Services
{
    public class PetGrpcServiceImpl: PetGrpcService.PetGrpcServiceBase
    {
        private readonly IPetService _petService;
        private readonly ILogger<PetGrpcServiceImpl> _logger;

        public PetGrpcServiceImpl(IPetService petService, ILogger<PetGrpcServiceImpl> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        public override async Task<PetResponse> GetPet(GetPetRequest request, ServerCallContext context)
        {
            var pet = await _petService.GetByIdAsync(request.Id);
            return new PetResponse
            {
                Id = pet.Id,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                BirthDate = pet.BirthDate.ToString("yyyy-MM-dd"),
                OwnerId = pet.OwnerId
            };
        }
        public override async Task<PetListResponse> GetAllPets (EmptyRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC GetAllPets called");
            var pets = await _petService.GetAllAsync();
            var response = new PetListResponse();
            foreach( var pet in pets)
            {
                response.Pets.Add(MapToResponse(pet));
            }
            _logger.LogInformation("Returning {Count} pets", response.Pets.Count);
            return response;
        }

        public override async Task<PetResponse> CreatePet(CreatePetRequest request, ServerCallContext context)
        {
            var petDto = new CreatePetDto
            {
                Name = request.Name,
                Species = request.Species,
                Breed = request.Breed,
                BirthDate = DateTime.Parse(request.BirthDate),
                OwnerId = request.OwnerId
            };

            var createdPet = await _petService.CreateAsync(petDto);
            return MapToResponse(createdPet);
        }
       
        public override async Task GetPetStream(
            EmptyRequest request, 
            IServerStreamWriter<PetResponse> responseStream, 
            ServerCallContext context)
        {
            var pets = await _petService.GetAllAsync();
            foreach (var pet in pets)
            {
                await responseStream.WriteAsync(MapToResponse(pet));
            }
        }

        private static PetResponse  MapToResponse(PetDto pet)
        {
            return new PetResponse
            {
                Id = pet.Id,
                Name = pet.Name ?? string.Empty,
                Species = pet.Species ?? string.Empty,
                Breed = pet.Breed ?? string.Empty,
                BirthDate = pet.BirthDate.ToString("yyyy-MM-dd"),
                OwnerId = pet.OwnerId,
                OwnerName = pet.OwnerName
            };
        }
    }
}