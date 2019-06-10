using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebAPI.Models.ResponseModels
{
    public class ChatMessageModel : ChatMessageCreate
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateRead { get; set; }
    }

  
}