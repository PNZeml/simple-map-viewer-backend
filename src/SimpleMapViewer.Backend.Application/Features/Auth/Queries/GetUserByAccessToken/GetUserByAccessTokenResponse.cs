using SimpleMapViewer.Backend.Application.Features.Auth.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.GetUserByAccessToken {
    internal class GetUserByAccessTokenResponse {
        public UserOutDto UserOut { get; set; }
    }
}