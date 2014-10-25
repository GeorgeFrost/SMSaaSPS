namespace SmsByPost.API.Request
{

    public class SmsMessageRequestData
    {
        public string characterset { get; set; }

        public string body { get; set; }
    }


    public class messages
    {
        public messages()
        {

        }

        public messages(SmsMessageRequestData messagesa)
        {
            message = messagesa;
        }

        public SmsMessageRequestData message { get; set; }
    }
}