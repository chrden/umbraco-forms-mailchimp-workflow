using Newtonsoft.Json;
using System.Collections.Generic;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Api.Response
{
    public class MailchimpListsResponse
    {
        [JsonProperty(PropertyName = "lists")]
        public IEnumerable<MailchimpList> Lists { get; set; }

        [JsonProperty(PropertyName = "total_items")]
        public int NumberOfLists { get; set; }

        public class MailchimpList
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            
            [JsonProperty(PropertyName = "web_id")]
            public int WebId { get; set; }
            
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }
    }
}