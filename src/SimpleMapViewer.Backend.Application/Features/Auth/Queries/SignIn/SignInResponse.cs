using System;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn {
    public class SignInResponse {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid AccessToken { get; set; }
    }
}