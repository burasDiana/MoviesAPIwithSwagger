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

    public class ChatMessageCreate
    {
        [Required(ErrorMessage = "Receiver id is required")]
        public int ReceiverId { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }

    public class ChatMessageResponseModel
    {
        public int RelatedUserId { get; set; }
        public List<ChatMessageModel> Messages { get; set; }
    }
}