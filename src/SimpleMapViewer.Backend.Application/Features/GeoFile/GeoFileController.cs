using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Dtos;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.ShareGeoFile;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.UploadGeoFile;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllGeoFiles;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetGeoFile;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile {
    public class GeoFileController : ApiBaseController {
        public GeoFileController(
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(taggedLifetimeScopeFactory) {
        }

        [HttpGet("{geoFileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetGeoFileAsync([FromRoute] long geoFileId) {
            try {
                await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
                var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

                var request = new GetGeoFileRequest {GeoFileId = geoFileId};
                var response = await mediator.Send(request);

                return Ok(response.GeoFile);
            } catch (ValidationException exception) {
                var problemDetails =
                    new ValidationProblemDetails(exception.Errors.FormatValidationErrors());
                return BadRequest(problemDetails);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IList<GeoFileOutDto>>> GetAllGeoFilesAsync(
            [FromQuery] bool shared
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new GetAllGeoFilesRequest {Shared = shared};
            var response = await mediator.Send(request);

            return Ok(response.GeoFiles);
        }

        [HttpGet("{geoFileId}/open")]
        public async Task<FileResult> OpenGeoFileAsync(
            [FromRoute] long geoFileId
        ) {
            // TODO
            var fileStream =
                new FileStream(@$"C:\ServerStorage\Files\1\{geoFileId}.geojson", FileMode.Open);

            return new FileStreamResult(fileStream, "application/octet-stream") {
                FileDownloadName = Path.GetFileName(fileStream.Name)
            };
        }

        [HttpPost]
        public async Task<ActionResult> UploadGeoFileAsync([FromForm(Name ="file")] IFormFile uploadingFile
        ) {
            // TODO
            /*if (uploadingFile == null) return Problem();
            var path = $"C:/ServerStorage/Files/{uploadingFile.FileName}";
            await using var fileStream = new FileStream(path, FileMode.Create);
            await uploadingFile.CopyToAsync(fileStream);*/
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new UploadGeoFileRequest();
            var response = await mediator.Send(request);

            return Ok();
        }

        [HttpPut("{geoFileId}/share")]
        public async Task<ActionResult> ShareGeoFileAsync(
            [FromRoute(Name = "GeoFileId")] long geoFileId,
            [FromBody] ShareGeoFileInDto shareGeoFileInDto
        ) {
            await using var unitOfWorkLifetimeScope = OpenUnitOfWorkScoop();
            var mediator = unitOfWorkLifetimeScope.Resolve<IMediator>();

            var request = new ShareGeoFileRequest {
                GeoFileId = geoFileId,
                UserIds = shareGeoFileInDto.UserIds,
                AccessType = shareGeoFileInDto.AccessType
            };
            await mediator.Send(request);

            return Ok();
        }

        [HttpDelete("{geoFileId}")]
        public async Task<ActionResult> DeleteGeoFileAsync(
            [FromQuery] long geoFileId
        ) {
            return Ok();
        }
    }
}