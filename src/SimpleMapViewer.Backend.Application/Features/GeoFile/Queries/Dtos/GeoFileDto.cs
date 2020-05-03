using System;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos {
    public class GeoFileDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public long Size { get; set; }
    }
}