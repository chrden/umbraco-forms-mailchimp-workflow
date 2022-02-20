using chrden.Umbraco.Forms.Workflows.Mailchimp.Models;
using chrden.Umbraco.Forms.Workflows.Mailchimp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Workflow
{
    public class MailchimpWorkflow : WorkflowType
    {
        private readonly IMailchimpService _mailchimpService;

        private FieldMapping _fieldMapping
        {
            get { return JsonConvert.DeserializeObject<FieldMapping>(FieldMapping); }
        }

        [Setting("Field mapping", Alias = "fieldMapping", Description = "Map Umbraco Form fields to the fields in a Mailchimp list", View = "~/App_Plugins/chrden.Umbraco.Forms.Workflows.Mailchimp/fieldmapping.html")]
        public string FieldMapping { get; set; }

        public MailchimpWorkflow(IMailchimpService mailchimpService)
        {
            _mailchimpService = mailchimpService;

            this.Id = new Guid("68bdc512-f67c-480f-921c-3a7a1b35c512");
            this.Name = "Push to Mailchimp";
            this.Description = "Map Umbraco Forms fields to the fields in a Mailchimp list";
            this.Icon = "icon-mindmap";
        }

        public override WorkflowExecutionStatus Execute(WorkflowExecutionContext context)
        {
            var emailAddressFormFieldAlias = _fieldMapping.FieldMappings.FirstOrDefault(m => m.ListFieldTag == "EMAIL").FormFieldAlias;

            var mergeFields =
                _fieldMapping.FieldMappings
                    //.Where(m => m.ListFieldTag != "EMAIL")
                    .Select(m => {
                        if (m.IsStatic)
                        {
                            return new KeyValuePair<string, string>(m.ListFieldTag, m.StaticValue);
                        }
                        else
                        {
                            return new KeyValuePair<string, string>(m.ListFieldTag, context.Record.GetValue<string>(m.FormFieldAlias));
                        }
                    })
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var success = 
                _mailchimpService.AddSubscriberToList(_fieldMapping.ListId, context.Record.GetValue<string>(emailAddressFormFieldAlias), mergeFields);

            return success ? WorkflowExecutionStatus.Completed : WorkflowExecutionStatus.Failed;
        }

        public override List<Exception> ValidateSettings()
        {
            var result = new List<Exception>();

            if (_fieldMapping == null || string.IsNullOrWhiteSpace(_fieldMapping.ListId))
            {
                result.Add(new Exception("Must choose a Mailchimp list"));
                return result;
            }

            if (!_fieldMapping.FieldMappings.Any())
                result.Add(new Exception("Must add field mappings"));

            if(!_fieldMapping.FieldMappings.Any(m => m.ListFieldTag == "EMAIL"))
                result.Add(new Exception("Must add a mapping for Email"));

            if (_fieldMapping.FieldMappings.Any(m => m.IsStatic && string.IsNullOrWhiteSpace(m.StaticValue)))
                result.Add(new Exception("Static mappings must have a value"));

            return result;
        }
    }
}