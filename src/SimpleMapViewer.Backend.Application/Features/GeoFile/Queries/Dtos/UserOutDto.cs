using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos {
    public class UserOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
        public AccessType? AccessType { get; set; }
    }
}