using System;
using System.Collections.Generic;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos {
    public class GeoFileOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Opened { get; set; }
        public long Size { get; set; }
        public string Owner { get; set; }
        public IList<UserOutDto> Users { get; set; }
        public IList<GeoFileActivityRecordOutDto> GeoFileActivityRecords { get; set; }
    }
}