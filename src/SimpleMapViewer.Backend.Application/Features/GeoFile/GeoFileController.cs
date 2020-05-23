using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Dtos;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Share;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Upload;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAll;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile {
    [Route("api/v1/")]
    [AllowAnonymous]
    public class GeoFilesController : ApiBaseController {
        public GeoFilesController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) { }
        // TODO
        [HttpGet("users/{UserId}/geofiles/{GeoFileId}")]
        public async Task<FileResult> OpenAsync(
            [FromRoute(Name = "UserId")] long userId,
            [FromRoute(Name = "GeoFileId")] long geoFileId
        ) {
            var fileStream = new FileStream(@"C:\ServerStorage\Files\1\jp_prefs.geojson", FileMode.Open);

            return new FileStreamResult(fileStream, "application/octet-stream") {
                FileDownloadName = Path.GetFileName(fileStream.Name)
            };
        }

        [HttpGet("users/{UserId}/geofiles")]
        public async Task<ActionResult<IList<GeoFileDto>>> GetAllAsync(
            [FromRoute(Name = "UserId")] long userId,
            [FromQuery(Name = "shared")] bool shared
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new GetAllRequest { UserId = userId, Shared = shared };
            var response = await mediator.Send(request);

            return Ok(response.GeoFiles);
        }

        // TODO
        //[HttpPost("users/{UserId}/geofiles/")]
        [HttpPost("create")]
        public async Task<IActionResult> UploadAsync(
            [FromForm(Name ="file")] IFormFile uploadingFile
        ) {
            /*if (uploadingFile == null) return Problem();
            var path = $"C:/ServerStorage/Files/{uploadingFile.FileName}";
            await using var fileStream = new FileStream(path, FileMode.Create);
            await uploadingFile.CopyToAsync(fileStream);*/
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new UploadRequest();
            var response = await mediator.Send(request);

            return Ok();
        }

        // TODO
        [HttpPut("users/{UserId}/geofiles/{GeoFileId}/share")]
        public async Task<ActionResult> ShareAsync(
            [FromRoute(Name = "UserId")] long userId,
            [FromRoute(Name = "GeoFileId")] long geoFileId,
            [FromBody] ShareOptionsDto shareOptionsDto
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new ShareRequest {
                UserId = userId,
                GeoFileId = geoFileId,
                UserIds = shareOptionsDto.UserIds,
                AccessType = shareOptionsDto.AccessType
            };
            await mediator.Send(request);

            return Ok();
        }

        // TODO
        [HttpDelete("users/{UserId}/geofiles/{GeoFileId}")]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute(Name = "UserId")] long userId,
            [FromRoute(Name = "GeoFileId")] long geoFileId
        ) {
            return Ok();
        }
    }
}