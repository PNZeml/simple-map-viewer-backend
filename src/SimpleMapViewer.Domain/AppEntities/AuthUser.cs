using System;

namespace SimpleMapViewer.Domain.AppEntities {
    public class AuthUser {
        public long Id { get; }
        public Guid? AccessToken { get; }

        public AuthUser(long id, Guid? accessToken) {
            Id = id;
            AccessToken = accessToken;
        }
    }
}