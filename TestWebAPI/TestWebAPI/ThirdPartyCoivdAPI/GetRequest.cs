using System;
using System.IO;
using System.Net;

namespace TestWebAPI.ThirdPartyCoivdAPI
{
    public class GetRequest
    {
        HttpWebRequest request;
        string adressUrl;

        public string Response { get; set; }

        public GetRequest(string adressUrl)
        {
            this.adressUrl = adressUrl;
        }

        public void Run()
        {
            request = (HttpWebRequest)WebRequest.Create(adressUrl);
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                if(stream != null) Response = new StreamReader(stream).ReadToEnd();
            }
            catch(Exception)
            {

            }
        }
    }
}
