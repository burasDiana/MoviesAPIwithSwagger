using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Examples;
using TestWebAPI.Models.ResponseModels;

namespace TestWebAPI.Models.ExampleModels
{
    public class ChatMessageModelExample : ChatMessageCreate, IExamplesProvider
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateRead { get; set; }

        public object GetExamples()
        {
            var current_year = DateTime.Now.ToString("yyyy");
            var current_month = DateTime.Now.AddMonths(1).ToString("MM");
            DateTime dateCreated = Convert.ToDateTime($"{current_year}-{current_month}-01T09:00:00");

            return new ChatMessageModelExample()
            {
                Id = 12456,
                SenderId = 87905,
                ReceiverId = 42587,
                DateCreated = dateCreated,
                DateRead = null,
                Message = "Have a nice day!"
            };
        }
    }

    public class ChatMessageCreateExample : ChatMessageCreate, IExamplesProvider
    {
        public object GetExamples()
        {
            return new ChatMessageCreateExample()
            {
                Message = "This is a great message to someone.",
                ReceiverId = 12346,
            };
        }
    }
}