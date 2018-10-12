using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    public class AuthReturnObject
    {
        public string TokenValue { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Type { get; set; }
        public string ExpiresIn { get; set; }

        public AuthReturnObject(string tokenValue, DateTime validFrom, DateTime validTo, string type, string expiresIn)
        {
            TokenValue = tokenValue;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Type = type;
            ExpiresIn = expiresIn;
        }
    }
}