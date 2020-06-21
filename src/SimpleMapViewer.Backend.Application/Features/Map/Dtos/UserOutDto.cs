namespace SimpleMapViewer.Backend.Application.Features.Map.Dtos {
    public class UserOutDto {
        public long Id { get; set; }
        // User's properties
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
        // Viewport properties
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Color { get; set; }
    }
}