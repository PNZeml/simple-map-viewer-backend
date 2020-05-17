using SimpleMapViewer.Backend.Application.Features.Auth.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.GetUserByAccessToken {
    internal class GetUserByAccessTokenResponse {
        public UserDto User { get; set; }
    }
}