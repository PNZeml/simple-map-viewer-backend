using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleMapViewer.Backend.Application.Features.Map.Dtos {
    public class UserInDto {
        public long Id { get; }
        public string ConnectionId { get; }
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

        private static readonly IList<int> colors = new List<int> {
            0xff4000, 0xffbf00, 0xbfff00, 0x00ff80, 0x0080ff, 0xff00bf
        };
        
        public UserInDto(long id, string connectionId) {
            Id = id;
            ConnectionId = connectionId;

            var rnd = new Random();
            Color = colors[rnd.Next(0, 5)];
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((UserInDto) obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, ConnectionId);
        }

        private bool Equals(UserInDto other) {
            return Id == other.Id && ConnectionId == other.ConnectionId;
        }
    }
}