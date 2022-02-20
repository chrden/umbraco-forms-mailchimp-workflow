namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Api.Response
{
    public class MailchimpErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public FieldErrors[] Errors { get; set; }

        public class FieldErrors
        {
            public string Field { get; set; }
            public string Message { get; set; }
        }
    }
}