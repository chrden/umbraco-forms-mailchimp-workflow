using Newtonsoft.Json;
using System.Collections.Generic;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Api.Request
{
    internal class MailchimpAddSubscriberRequest
    {
        [JsonProperty(PropertyName = "members")]
        public List<MemberDetails> Members { get; set; }

        public class MemberDetails
        {

            [JsonProperty(PropertyName = "status")]
            string Status => "subscribed";

            [JsonProperty(PropertyName = "status_if_new")]
            string StatusIfNew => "subscribed";

            [JsonProperty(PropertyName = "email_address")]
            public string EmailAddress { get; set; }

            [JsonProperty(PropertyName = "merge_fields")]
            public Dictionary<string, string> MergeFields { get; set; }
        }
    }
}