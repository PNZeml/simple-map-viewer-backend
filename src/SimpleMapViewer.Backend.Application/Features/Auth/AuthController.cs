using System;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp;
using SimpleMapViewer.Backend.Application.Features.Auth.Dtos;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn;

namespace SimpleMapViewer.Backend.Application.Features.Auth {
    [Route("api/v1")]
    [AllowAnonymous]
    public class AuthController : ApiBaseController {
        public AuthController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) { }

        [Route("signin")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> SignInAsync(
            [FromBody] SignInRequest request
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();
            var response = await mediator.Send(request);
            return Ok(response.User);
        }

        [Route("signup")]
        [HttpPost]
        public async Task<ActionResult> SignUpAsync(
            [FromBody] SignUpRequest request
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();
            await mediator.Send(request);
            return Ok();
        }
    }
}