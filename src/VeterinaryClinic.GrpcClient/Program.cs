// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using VeterinaryClinic.Grpc.Protos;

Console.WriteLine("Cliente gRPC - Veterinary Clinic");

using var channel = GrpcChannel.ForAddress("http://localhost:5199");

var client = new PetGrpcService.PetGrpcServiceClient(channel);

try
{
    Console.WriteLine("Prueba 1: Get All Pets Unary");
    var allPetsResponse = await client.GetAllPetsAsync(new EmptyRequest());
    Console.WriteLine($"Pets: {allPetsResponse.Pets.Count}");

    foreach (var pet in allPetsResponse.Pets)
    {
        Console.WriteLine($"Pet: {pet.Name}, Name: {pet.Name}");
        
    }
    Console.WriteLine();
    Console.WriteLine("Prueba 2: Get Pet By Id Unary");
    Console.WriteLine("Ingrese el Id de la mascota: ");
    var inputPetId = Console.ReadLine();
    if(int.TryParse(inputPetId, out int petId) ){
        var petByIdResponse = await client.GetPetAsync(new GetPetRequest { Id = petId });
        Console.WriteLine($"Pet: {petByIdResponse.Id}, Name: {petByIdResponse.Name}");
    }
    Console.WriteLine();

    Console.WriteLine("Prueba 3: GetStream");
    Console.WriteLine("Recibiendo mascotas una por una...");

    using var streamingCall = client.GetPetStream(new EmptyRequest());
    
    await foreach (var pet in streamingCall.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"[Stram] Pet: {pet.Id}, Name: {pet.Name}");
    }


}catch(RpcException ex)
{
    Console.WriteLine($"Error: {ex.Status.Detail}");
} catch(Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");  
}

Console.WriteLine("Presione cualquier tecla para salir...");
Console.ReadKey();