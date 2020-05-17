using System;
using Shura.Domain.Entities;

namespace SimpleMapViewer.Domain.Entities {
    public class User : Entity<long>, IHasIsDeleted {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHashed { get; set; }
        public virtual byte[] Avatar { get; set; }
        public virtual Guid AccessToken { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}