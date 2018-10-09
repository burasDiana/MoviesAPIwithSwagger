using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    public class Student : Person
    {
        public string ClassId { get; set; }
        public string ParentPhoneNr { get; set; }
    }
}