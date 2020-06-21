using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using SimpleMapViewer.Backend.Application.Features.Map.Controller;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Map.Hub {
    internal class MapHub : Hub<IMapClientRouter> {
        private readonly IMapHubController _controller;

        public MapHub(IMapHubController controller) {
            _controller = controller;
        }

        public Task Join(long geoFileId) {
            var authUser = Context.User.ToAuthUser();
            return _controller.AddUser(
                new UserInDto(authUser.Id, Context.ConnectionId),
                geoFileId
            );
        }

        public Task UpdateViewport(double x, double y, double height, double width) {
            return _controller.UpdateUserViewport(
                Context.ConnectionId,
                x, y, height, width
            );
        }
        
        public Task CreateComment(double x, double y, string comment) {
            var authUser = Context.User.ToAuthUser();
            return _controller.CreateComment(authUser, x, y, comment);
        }

        public async Task RemoveComment(long geoFileCommentId) {
            
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            await _controller.RemoveUser(Context.ConnectionId);
        }
    }
}