using System.Collections.Generic;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Models
{
    internal class FieldMapping
    {
        public string ListId { get; set; }
        public IEnumerable<FieldMap> FieldMappings { get; set; }

        public class FieldMap
        {
            public string FormFieldAlias { get; set; }
            public string StaticValue { get; set; }
            public bool IsStatic { get; set; }
            public string ListFieldTag { get; set; }
        }
    }
}