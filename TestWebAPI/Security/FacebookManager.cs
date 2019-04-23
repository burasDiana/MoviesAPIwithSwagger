using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace TestWebAPI.Security
{
    public class FacebookManager
    {
        private const string FbGraphUrlBase = "https://graph.facebook.com/v3.2/"; //base url for fb graph requests
        private const string UserAcessToken = "EAAWEtCJRvrsBAN0qVMlb5gH1uKyjjeKSMEHIMpG6hrH0qtMe6fHdk1mEZASGg5ZAAGxOC54Cqgjp9xt6MaPioCz7JIcdy3pvB7ig8xaDd1hxJwYHFxFqls5oyxqaLcnMRdC35WlJMUT6leqg34kxph8ZAIEZCaQZD"; //generated user token for an app
        private const string AppAccessToken = "1553284088315579|j-miucxfMAcAPKHjMO8Lz_reu7I"; //found in facebook access token tool at https://developers.facebook.com/tools/access_token/
        private const string AdminEmail = "testemail@email.com";
        private const string descBase = "Facebook webhook: ";
        private static string fbPageToken = "";

        /// <summary>
        /// This method validates the given user token with the facebook graph api and requests a one time use page token
        /// </summary>
        public static bool GetPageAccessToken()
        {
            try
            {
                // verify authenticity of user by checking the token
                // if response is 200 OK, token is authentic and valid
                var urlUserToken =
                    $"{FbGraphUrlBase}debug_token?input_token={UserAcessToken}&access_token={AppAccessToken}"; // example https://graph.facebook.com/v3.2/debug_token?input_token=EAN2343jnfkwm23eojdm&access_token=ERASSvfjiewfvmeqpdlxw

                var response = RequestGetResponse(urlUserToken).First();

                // user is valid at this point -> request page token 
                var urlPageToken = $"{FbGraphUrlBase}me/accounts?access_token={UserAcessToken}"; // example https://graph.facebook.com/v3.2/me/accounts?access_token=ERASSvfjiewfvmeqpdlxw

                var response2 = RequestGetResponse(urlPageToken).First();

                if (response2.Key == HttpStatusCode.OK)
                {
                    var responseParsed = JObject.Parse(response2.Value);
                    fbPageToken = responseParsed["data"][0]["access_token"].ToString();
                }
            }
            catch (WebException e)
            {
                var responseContent = "";
                var description = "";
                using (WebResponse response = e.Response)
                {
                    description = response.ResponseUri.ToString().Contains("debug") ? "User access token error" : "Page access token error";
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        responseContent = streamReader.ReadToEnd();
                }
                HandleBadResponse(responseContent, description);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sends a web request to a uri and returns a key-value pair with the status code and response body
        /// </summary>
        private static Dictionary<HttpStatusCode, string> RequestGetResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse http_response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(http_response.GetResponseStream());
            string response = sr.ReadToEnd();
            return new Dictionary<HttpStatusCode, string> { { http_response.StatusCode, response } };
        }

        /// <summary>
        /// Gets the error message from the response and sends a mail to admin users
        /// </summary>
        private static void HandleBadResponse(string response_content, string description)
        {
            var responseParsed = JObject.Parse(response_content);
            var errorMessage = responseParsed["error"]["message"].ToString(); //Facebook always sends an error message
            SendMailToAdmins(errorMessage, description);
        }

        /// <summary>
        /// Sends a mail to an admin
        /// </summary>
        public static void SendMailToAdmins(string error, string description)
        {
            
        }
    }
}