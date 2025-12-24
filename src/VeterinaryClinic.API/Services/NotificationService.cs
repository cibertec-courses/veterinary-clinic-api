
using Microsoft.AspNetCore.SignalR;
using VeterinaryClinic.API.Hubs;

namespace VeterinaryClinic.API.Services
{

    public interface INotificationService
    {
        Task NotifyAppointmentCreated(int appointmentId, string petName, DateTime appointmentDate);
        Task NotifyAppointmentUpdated(int appointmentId, string petName, string status);
        Task NotifyPetCreated(int petId, string petName, string ownerName );   
        Task NotifyToGroup(string group, string title, string message);
        
    }  


    public class NotificationService: INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext ,ILogger<NotificationService> logger, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyAppointmentCreated(int appointmentId, string petName, DateTime appointmentDate)
        {
            _logger.LogInformation("Notificando cita creada");
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
            {
                Type = "AppointmentCreated",
                Title = "Nueva cita creada",
                Message = $"Se ha creado una nueva cita para la mascota {petName} el {appointmentDate.ToString("dd/MM/yyyy")}",
                Data = new { AppointmentId = appointmentId, PetName = petName, Date = appointmentDate },
                TimeSpan = DateTime.UtcNow
            }); 
        }

        public async Task NotifyAppointmentUpdated(int appointmentId, string petName, string status)
        {
            _logger.LogInformation("Notificando cita actualizada");
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
            {
                Type = "AppointmentUpdated",
                Title = "Cita actualizada",
                Message = $"La cita de la mascota {petName} ha sido actualizada a {status}",
                Data = new { AppointmentId = appointmentId, PetName = petName, Status = status },
                TimeSpan = DateTime.UtcNow
            });
        }

        public async Task NotifyPetCreated(int petId, string petName, string ownerName)
        {
            _logger.LogInformation("Notificando mascota creada");
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
            {
                Type = "PetCreated",
                Title = "Nueva mascota creada",
                Message = $"Se ha creado una nueva mascota {petName} para el dueño {ownerName}",
                Data = new { PetId = petId, PetName = petName, OwnerName = ownerName },
                TimeSpan = DateTime.UtcNow
            });
        }

        public async Task NotifyToGroup(string group, string title, string message)
        {
            _logger.LogInformation("Notificando a grupo");

            await _hubContext.Clients.Group(group).SendAsync("ReceiveNotification", new
            {
                Type = "GroupNotification",
                Title = title,
                Message = message,
                Group = group,
                TimeSpan = DateTime.UtcNow
            });
        }
    }

}