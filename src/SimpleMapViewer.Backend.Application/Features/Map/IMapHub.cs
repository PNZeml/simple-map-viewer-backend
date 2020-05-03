using System.Threading.Tasks;

namespace SimpleMapViewer.Backend.Application.Features.Map {
    public interface IMapHub {
        Task ReceiveMessage(MapMessageDto mapMessageDto);
    }
}