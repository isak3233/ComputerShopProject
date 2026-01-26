using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webbshop.Models
{
    internal class LoginAttempt
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public int LoginAttemptsAmount { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime LastLogonDate { get; set; }
    }
}
