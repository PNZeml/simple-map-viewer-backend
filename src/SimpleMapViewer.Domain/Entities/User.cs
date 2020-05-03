using System;
using System.Collections.Generic;

namespace SimpleMapViewer.Domain.Entities {
    public class User {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHashed { get; set; }
        public virtual Guid AccessToken { get; set; }  
        public virtual IList<GeoFile> OwnedGeoFiles { get; set; }
    }
}