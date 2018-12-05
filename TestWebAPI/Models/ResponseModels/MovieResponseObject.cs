﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models.ResponseModels
{
    public class MovieResponseObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}