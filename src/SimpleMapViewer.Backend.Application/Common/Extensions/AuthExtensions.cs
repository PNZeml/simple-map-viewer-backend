using System;
using System.Linq;
using System.Security.Claims;
using FluentNHibernate.Conventions;
using SimpleMapViewer.Domain.AppEntities;

namespace SimpleMapViewer.Backend.Application.Common.Extensions {
    public static class AuthExtensions {
        public static AuthUser ToAuthUser(this ClaimsPrincipal claimsPrincipal) {
            if (claimsPrincipal.Claims.IsEmpty()) return null;

            var idClaim = claimsPrincipal.Claims.Single(x => x.Type == nameof(AuthUser.Id));
            var id = long.Parse(idClaim.Value);
            var accessTokenClaim = claimsPrincipal.Claims
                .SingleOrDefault(x => x.Type == nameof(AuthUser.AccessToken));
            var authToken = Guid.TryParse(accessTokenClaim?.Value, out var token)
                ? token
                : (Guid?)null;
            
            return new AuthUser(id, authToken);
        }
    }
}