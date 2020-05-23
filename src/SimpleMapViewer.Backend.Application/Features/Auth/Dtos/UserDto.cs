namespace SimpleMapViewer.Backend.Application.Features.Auth.Dtos {
    public class UserDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
        public string AccessToken { get; set; }
    }
}