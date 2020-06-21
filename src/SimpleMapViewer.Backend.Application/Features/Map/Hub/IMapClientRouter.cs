using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Map.Hub {
    public interface IMapClientRouter {
        Task UpdateUsers(IList<UserOutDto> users);
        Task UpdateComments(IList<GeoFileCommentOutDto> geoFileComment);
    }
}