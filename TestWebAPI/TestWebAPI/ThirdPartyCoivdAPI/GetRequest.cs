namespace TestWebAPI.ThirdPartyCoivdAPI
{
    public class GetRequest
    {
        string addressUrl;

        public string? responseData { get; set; }

        public GetRequest(string addressUrl)
        {
            this.addressUrl = addressUrl;
        }

        public async void Run()
        {
            var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            try
            {
                var response = await client.GetAsync(addressUrl);
                var streamResponse = await response.Content.ReadAsStringAsync();
                if (streamResponse != null)
                {
                    responseData = new StreamReader(streamResponse).ReadToEnd();
                }
            }
            catch(Exception)
            {

            }
        }
    }
}
