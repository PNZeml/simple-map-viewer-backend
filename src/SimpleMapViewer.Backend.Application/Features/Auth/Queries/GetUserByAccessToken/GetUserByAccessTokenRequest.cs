using System;
using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.GetUserByAccessToken {
    [UnitOfWorkRequired]
    internal class GetUserByAccessTokenRequest : IRequest<GetUserByAccessTokenResponse> {
        public Guid AccessToken { get; set; }
    }
}