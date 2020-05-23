using MediatR;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Event.GeoFileActivityOccured {
    internal class GeoFileActivityOccuredNotification : INotification {
        public long GeoFileId { get; set; }
        public long UserId { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}