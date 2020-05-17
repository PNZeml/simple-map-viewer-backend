using System;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos {
    public class GeoFileDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Opened { get; set; }
        public long Size { get; set; }
    }
}