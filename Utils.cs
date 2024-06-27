namespace Proyecto2Api
{
    public class Utils
    {
        private readonly string _apiHost;
        private readonly string _emailAPI;

        public Utils(IConfiguration configuration)
        {
            _apiHost = configuration.GetValue<string>("APIHost");
            _emailAPI = configuration.GetValue<string>("EmailAPI");
        }

        public string GetEmailAPI()
        {
            return _emailAPI;
        }

        public HttpClient GetAPIHost()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_apiHost);
            return client;
        }
    }
}
