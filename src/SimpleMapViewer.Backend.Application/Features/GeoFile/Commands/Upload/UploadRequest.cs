using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Upload {
    [UnitOfWorkRequired]
    public class UploadRequest : IRequest { }
}