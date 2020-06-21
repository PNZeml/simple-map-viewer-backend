using System;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos {
    public class GeoFileActivityRecordOutDto {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public byte[] Avatar { get; set; }
        public ActivityType ActivityType { get; set; }
        public DateTime Occurred { get; set; }
    }
}