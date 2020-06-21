using System;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.Dtos;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn;

namespace SimpleMapViewer.Backend.Application.Features.Auth {
    [Route("api/v1")]
    [AllowAnonymous]
    public class AuthController : ApiBaseController {
        private readonly IMapper _mapper;

        public AuthController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory,
            IMapper mapper
        ) : base(taggedLifetimeScopeFactory) {
            _mapper = mapper;
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserOutDto>> SignInAsync(
            [FromBody] SignInDto signInDto
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = _mapper.Map<SignInRequest>(signInDto);
            var response = await mediator.Send(request);

            if (response.User == null) {
                var problemDetails = new ProblemDetails {
                    Title = "Unauthorized",
                    Detail = "Invalid login or password"
                };
                return Unauthorized(problemDetails);
            }
            
            return Ok(response.User);
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> SignUpAsync(
            [FromBody] SignUpDto signUpDto
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = _mapper.Map<SignUpRequest>(signUpDto);
            await mediator.Send(request);

            return Ok();
        }
    }
}