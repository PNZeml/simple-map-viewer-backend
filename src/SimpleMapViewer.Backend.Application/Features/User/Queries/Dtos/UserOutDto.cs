namespace SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos {
    public class UserOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
    }
}