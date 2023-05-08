namespace OAuth.Custom.Github.WebClient
{
    public class CallbackResponse
    {
        public string State { get; set; }
        public string AuthCode { get; set; }
    }
}
