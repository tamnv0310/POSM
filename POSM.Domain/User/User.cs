using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSM.Domain.User
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public POSMRole Role { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DaletedDateUtc { get; set; }
        public Guid? SupervisorId { get; set; }

        public virtual User Supervior { get; set; }
        public virtual List<User> Supervisees { get; set; }
    }
}
