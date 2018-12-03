using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    public class ResetPasswordRequest
    {
        public string UserName { get; set; }

        public string SecretCode { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string EmailAddress { get; set; }
    }
}