using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CD.UmbracoFormsMailchimpWorkflow.Models.Response.MergeFields
{
    public class MailchimpMergeFieldsResponse
    {
        public IEnumerable<Merge_Field> merge_fields { get; set; }
        public string list_id { get; set; }
        public int total_items { get; set; }
        public IEnumerable<_Links> _links { get; set; }

    }

    public class Merge_Field
    {
        public int merge_id { get; set; }
        public string tag { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool required { get; set; }
        public string default_value { get; set; }
        public bool _public { get; set; }
        public int display_order { get; set; }
        public Options options { get; set; }
        public string help_text { get; set; }
        public string list_id { get; set; }
        public _Links[] _links { get; set; }
    }

    public class Options
    {
        public int size { get; set; }
    }

    public class _Links
    {
        public string rel { get; set; }
        public string href { get; set; }
        public string method { get; set; }
        public string targetSchema { get; set; }
        public string schema { get; set; }
    }
}