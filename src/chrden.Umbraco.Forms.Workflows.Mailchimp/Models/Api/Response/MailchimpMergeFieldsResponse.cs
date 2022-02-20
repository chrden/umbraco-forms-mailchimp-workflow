using Newtonsoft.Json;
using System.Collections.Generic;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Api.Response
{
    public class MailchimpMergeFieldsResponse
    {
        [JsonProperty(PropertyName = "merge_fields")]
        public IEnumerable<MergeField> MergeFields { get; set; }

        [JsonProperty(PropertyName = "list_id")]
        public string ListId { get; set; }

        [JsonProperty(PropertyName = "total_items")]
        public int NumberOfFields { get; set; }

        public class MergeField
        {
            [JsonProperty(PropertyName = "merge_id")]
            public int MergeId { get; set; }

            public string Tag { get; set; }

            public string Name { get; set; }

            public bool Required { get; set; }

            [JsonProperty(PropertyName = "default_value")]
            public string DefaultValue { get; set; }

            public bool Public { get; set; }
        }
    }
}