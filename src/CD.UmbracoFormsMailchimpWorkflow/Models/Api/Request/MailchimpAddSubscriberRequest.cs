using Newtonsoft.Json;
using System.Collections.Generic;

namespace CD.UmbracoFormsMailchimpWorkflow.Models.Api.Request
{
    internal class MailchimpAddSubscriberRequest
    {
        [JsonProperty(PropertyName = "status")]
        string Status => "subscribed";

        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "merge_fields")]
        public Dictionary<string, string> MergeFields { get; set; }
    }
}