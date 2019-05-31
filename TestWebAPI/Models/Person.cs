using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    // This class defines the properties of a person object
    public abstract class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
