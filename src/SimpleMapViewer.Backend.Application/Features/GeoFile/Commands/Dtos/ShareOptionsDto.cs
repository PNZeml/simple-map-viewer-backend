using System.Collections.Generic;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Dtos {
    public class ShareOptionsDto {
        public IList<long> UserIds { get; set; }
        public AccessType AccessType { get; set; }
    }
}