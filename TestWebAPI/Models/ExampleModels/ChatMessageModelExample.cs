using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Examples;
using TestWebAPI.Models.ResponseModels;

namespace TestWebAPI.Models.ExampleModels
{

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