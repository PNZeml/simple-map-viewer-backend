using System.Threading.Tasks;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Map.Hubs.MapHub {
    public interface IMapHub {
        Task ReceiveMessage(MapMessageDto mapMessageDto);
    }
}