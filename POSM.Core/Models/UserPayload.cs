using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSM.Core.Models
{
    public class UserPayload
    {
        public Guid UserId { get; set; }

        public DateTime ExpiredInUtc { get; set; }

        public bool IsRefreshToken { get; set; }
    }
}
