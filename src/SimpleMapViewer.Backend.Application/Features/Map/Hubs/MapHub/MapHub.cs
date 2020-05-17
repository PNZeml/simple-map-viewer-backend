using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Map.Hubs.MapHub {
    public class MapHub : Hub<IMapHub> {
        public Task SendMessage(double x, double y, string message) {
            return Clients.All.ReceiveMessage(
                new MapMessageDto {X = x, Y = y, Message = message}
            );
        }
    }
}