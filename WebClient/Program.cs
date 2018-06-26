using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = new Task(HTTP_GET);
            t.Start();
            Console.ReadLine();
        }

        static async void HTTP_GET()
        {
            var TARGETURL = "http://localhost:53565/api/movies";

            HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://127.0.0.1:8888") ,
                UseProxy = true ,
            };

            Console.WriteLine("GET: + " + TARGETURL);

            // ... Use HttpClient.            
            HttpClient client = new HttpClient(handler);

            var byteArray = Encoding.ASCII.GetBytes("Alex:2LO1AASEOXUQP5LJ8KOFYW5:7b5ddca3a6084980ab48a840faa23c0b");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic" , Convert.ToBase64String(byteArray));
            client.DefaultRequestHeaders.Add("Accept-Version" , "3.0");
            HttpResponseMessage response = await client.GetAsync(TARGETURL);
            HttpContent content = response.Content;

            // ... Check Status Code                                
            Console.WriteLine("Response StatusCode: " + (int) response.StatusCode + " " + response.ReasonPhrase);

            // ... Read the string.
            string result = await content.ReadAsStringAsync();

            // ... Display the result.
            if ( result != null)
            {
                Console.WriteLine(result);
            }
        }
    }
}
