using System.Collections.Generic;
using MediatR;
using SimpleMapViewer.Domain.Enums;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.ShareGeoFile {
    [UnitOfWorkRequired]
    public class ShareGeoFileRequest : IRequest {
        public long GeoFileId { get; set; }
        public IList<long> UserIds { get; set; }
        public AccessType AccessType { get; set; }
    }
}