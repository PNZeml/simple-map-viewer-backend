using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllByGeoFile;
using SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllBySearchQuery;

namespace SimpleMapViewer.Backend.Application.Features.User {
    [Route("api/v1/")]
    [AllowAnonymous]
    public class UserController : ApiBaseController {
        public UserController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) { }

        [HttpGet("users")]
        public async Task<ActionResult<IList<UserDto>>> GetAllBySearchQueryAsync(
            [FromQuery] GetAllBySearchQueryRequest searchQueryRequest
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var response = await mediator.Send(searchQueryRequest);

            return Ok(response.Users);
        }

        [HttpGet("users/{UserId}/geofiles/{GeoFileId}/share")]
        public async Task<ActionResult<IList<UserAccessTypeDto>>> GetAllByGeoFileAsync(
            [FromRoute(Name = "UserId")] long userId,
            [FromRoute(Name = "GeoFileId")] long geoFileId
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new GetAllByGeoFileRequest { GeoFileId = geoFileId };
            var response = await mediator.Send(request);
            
            return Ok(response.Users);
        }
    }
}