using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.UploadGeoFile {
    [UnitOfWorkRequired]
    public class UploadGeoFileRequest : IRequest { }
}