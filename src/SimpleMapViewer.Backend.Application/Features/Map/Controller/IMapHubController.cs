using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;
using SimpleMapViewer.Domain.AppEntities;

namespace SimpleMapViewer.Backend.Application.Features.Map.Controller {
    internal interface IMapHubController {
        Task AddUser(UserInDto user, long geoFileId);
        Task RemoveUser(string connectionId);
        Task UpdateUserViewport(
            string connectionId,
            double x,
            double y,
            double height,
            double width
        );
        Task CreateComment(AuthUser authUser, double x, double y, string comment);
    }
}