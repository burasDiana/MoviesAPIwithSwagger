using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace TestWebAPI.Security
{
    public class AppSettings
    {
        // OneSignal
        public static string OneSignalIOSAppId => (FindValue<string>("OneSignalIOSAppId"));
        public static string OneSignalAndroidAppId => (FindValue<string>("OneSignalAndroidAppId"));
        public static string OneSignalIOSRestApiKey => (FindValue<string>("OneSignalIOSRestApiKey"));
        public static string OneSignalAndroidRestApiKey => (FindValue<string>("OneSignalAndroidRestApiKey"));
        public static string OneSignalRestApiUrl => (FindValue<string>("OneSignalRestApiUrl"));

        /// <summary>
        /// Returns the value of a setting with the specified name.
        /// </summary>
        private static T FindValue<T>(string name)
        {
            //TODO implement this
            // get settings list from db
            // filter by name and return value
            var new_string = "apple";
            return (T)Convert.ChangeType(new_string, typeof(T));
        }
    }

    public class PushNotifcationsManager
    {
        private static string OneSignalApiToken = "";
        private static string OneSignalAppId = "";

        /// <summary>
        /// Sends a push notification to a device id via OneSignal
        /// </summary>
        public static void SendPushNotificationToMobileDevice(string userId, string message, string platform, out bool success)
        {
            SetPlatformSpecificData(platform);

            success = false;

            string title = "Ny besked"; //TODO change this accordingly

            string json = GetJsonContent(userId, message, title);

            if (CreatePostRequest(json).First().Key == HttpStatusCode.OK)
            {
                success = true;
            }
            //create apievent

            //save push notification to db
        }

        /// <summary>
        /// Create a Post request to the OneSignal Rest API 
        /// </summary>
        private static Dictionary<HttpStatusCode, string> CreatePostRequest(string jsonContent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(AppSettings.OneSignalRestApiUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Headers.Add("Authorization", "Basic " + AppSettings.OneSignalIOSRestApiKey);
            
            string responseContent = null;
            HttpWebResponse response = null;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonContent);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                response = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseContent = streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                var error = e.Message;
                responseContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                HandleBadResponse(responseContent, error);
            }
            return new Dictionary<HttpStatusCode, string> { { response.StatusCode, responseContent } };
        }

        /// <summary>
        /// Creates the json payload for OneSignal notification requests
        /// </summary>
        private static string GetJsonContent(string userId, string message, string title)
        {
            string json = new JavaScriptSerializer().Serialize(new
            {
                app_id = OneSignalAppId,
                include_player_ids = new[] { userId },
                contents = new
                {
                    en = message
                },
                headings = new
                {
                    en = title
                }
            });
            return json;
        }

        private static void HandleBadResponse(string responseContent, string description)
        {
            var responseParsed = JObject.Parse(responseContent);
            var errorMessage = responseParsed["errors"].ToString();
            SendMailToAdmins(errorMessage, description);
        }


        /// <summary>
        /// Sends an email to admins if the request fails
        /// </summary>
        private static void SendMailToAdmins(string error, string description)
        {
           //
        }

        /// <summary>
        /// This method sets the appropriate OneSignal variables for a specific platform
        /// </summary>
        private static void SetPlatformSpecificData(string platform)
        {
            bool isIOS = platform.Contains("IOS");
            OneSignalApiToken = isIOS ? AppSettings.OneSignalIOSRestApiKey : AppSettings.OneSignalAndroidRestApiKey;
            OneSignalAppId = isIOS ? AppSettings.OneSignalIOSAppId : AppSettings.OneSignalAndroidAppId;
        }
    }
}