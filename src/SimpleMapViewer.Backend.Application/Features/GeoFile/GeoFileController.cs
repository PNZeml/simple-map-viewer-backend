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
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAll;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile {
    [Route("api/v1/")]
    [AllowAnonymous]
    public class GeoFilesController : ApiBaseController {
        public GeoFilesController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) { }

        [HttpGet("users/{UserId}/geofiles")]
        public async Task<ActionResult<IList<GeoFileDto>>> GetAllAsync(
            [FromRoute(Name = "UserId")] long userId
        ) {
            await using var unitOfWorkLifetimeScope = UnitOfWorkLifetimeScope;
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var response = await mediator.Send(new GetAllRequest { UserId = userId});

            return Ok(response.GeoFiles);
        }

        [HttpGet("users/{UserId}/geofiles/{FileId}")]
        public async Task<FileResult> GetByIdAsync(
            [FromRoute(Name = "UserId")] long userId,
            [FromRoute(Name = "FileId")] long fileId
        ) {
            var fileStream = new FileStream(@"C:\ServerStorage\Files\1\jp_prefs.geojson", FileMode.Open);
            return new FileStreamResult(fileStream, "application/octet-stream") {
                FileDownloadName = Path.GetFileName(fileStream.Name)
            };
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(
            [FromForm(Name ="file")] IFormFile uploadingFile
        ) {
            try {
                if (uploadingFile == null) return Problem();
                var path = $"C:/ServerStorage/Files/{uploadingFile.FileName}";
                await using var fileStream = new FileStream(path, FileMode.Create);
                await uploadingFile.CopyToAsync(fileStream);
                return Ok();
            } catch (Exception exception) {
                return StatusCode(500);
            }
        }
    }
}