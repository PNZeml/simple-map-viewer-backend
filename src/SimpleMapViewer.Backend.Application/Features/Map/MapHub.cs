using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SimpleMapViewer.Backend.Application.Features.Map {
    public class MapHub : Hub<IMapHub> {
        public Task SendMessage(double x, double y, string message) {
            return Clients.All.ReceiveMessage(
                new MapMessageDto {X = x, Y = y, Message = message}
            );
        }
    }
}