using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsers;

namespace SimpleMapViewer.Backend.Application.Features.User {
    public class UserController : ApiBaseController {
        private readonly IMapper _mapper;

        public UserController(
            IMapper mapper,
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserOutDto>>> GetAllUsersAsync(
            [FromQuery] GetAllUsersInDto getAllUsersInDto
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = _mapper.Map<GetAllUsersRequest>(getAllUsersInDto);
            var response = await mediator.Send(request);

            return Ok(response.Users);
        }
    }
}