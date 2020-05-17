using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.User.Dtos;
using SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsersBy;

namespace SimpleMapViewer.Backend.Application.Features.User {
    public class UserController : ApiBaseController {
        public UserController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) { }

        [HttpGet]
        public async Task<ActionResult<IList<UserDto>>> GetAllByAsync(
            [FromQuery] GetAllUsersByRequest request
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();
            var response = await mediator.Send(request);
            return Ok(response.Users);
        }
    }
}