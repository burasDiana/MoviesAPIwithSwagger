using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models
{
    public class MoviePatchRequest
    {
        public decimal ?Price { get; set; }
    }
}