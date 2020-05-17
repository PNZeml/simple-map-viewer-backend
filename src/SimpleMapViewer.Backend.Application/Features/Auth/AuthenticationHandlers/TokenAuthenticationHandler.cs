using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.GetUserByAccessToken;
using SimpleMapViewer.Infrastructure.Constants;

namespace SimpleMapViewer.Backend.Application.Features.Auth.AuthenticationHandlers {
    internal class TokenAuthenticationHandler :
        AuthenticationHandler<AuthenticationSchemeOptions> {
        private AuthFailCause AuthFailResponse { get; set; }

        private readonly Func<object, ILifetimeScope> _taggedLifetimeScopeFactory;

        public TokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(options, logger, encoder, clock) {
            _taggedLifetimeScopeFactory = taggedLifetimeScopeFactory;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            if (!TryRetrieveTokenFromQueryString(out var accessToken)) {
                if (!TryRetrieveTokenFromHeader(out accessToken)) {
                    AuthFailResponse = AuthFailCause.TokenIsNotProvided;
                    return AuthenticateResult.Fail(AuthFailResponse.Message);
                }
            }

            if (!Guid.TryParse(accessToken, out var accessTokenGuid)) {
                AuthFailResponse = AuthFailCause.InvalidToken;
                return AuthenticateResult.Fail(AuthFailResponse.Message);
            }

            await using var unitOfWorkLifetimeScope = 
                _taggedLifetimeScopeFactory(LifetimeScopeTags.UNIT_OF_WORK);
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();
            var request = new GetUserByAccessTokenRequest { AccessToken = accessTokenGuid };
            var response = await mediator.Send(request);
            var user = response.User;

            if (user == null) {
                AuthFailResponse = AuthFailCause.InvalidToken;
                return AuthenticateResult.Fail(AuthFailResponse.Message);
            }

            var identity = new ClaimsIdentity(Scheme.Name);
            identity.AddClaim(new Claim(
                nameof(Domain.Entities.User.Id),
                user.Id.ToString()
            ));
            identity.AddClaim(new Claim(
                nameof(Domain.Entities.User.AccessToken),
                user.AccessToken
            ));
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        
        private bool TryRetrieveTokenFromQueryString(out string token) {
            token = string.Empty;

            var queryParams = QueryHelpers.ParseQuery(Request.QueryString.Value);
            if (!queryParams.TryGetValue("access_token", out var accessToken)) return false;

            token = accessToken.ToString();
            return true;
        }

        private bool TryRetrieveTokenFromHeader(out string token) {
            token = string.Empty;

            if (!Request.Headers.ContainsKey("Authorization")) return false;

            AuthenticationHeaderValue.TryParse(
                Request.Headers[HeaderNames.Authorization],
                out var authHeader
            );

            if (authHeader?.Parameter is null || authHeader?.Scheme is null) return false;

            token = authHeader.Parameter;
            return true;
        }
        
        private struct AuthFailCause {
            public int Code { get; }
            public string Message { get; }

            public static readonly AuthFailCause TokenIsNotProvided =
                new AuthFailCause(0, "A token is not provided");
            public static readonly AuthFailCause InvalidToken =
                new AuthFailCause(1, "An invalid token");

            private AuthFailCause(int code, string message) {
                Code = code;
                Message = message;
            }
        }
    }
}