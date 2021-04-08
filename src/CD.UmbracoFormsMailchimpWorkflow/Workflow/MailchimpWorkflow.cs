﻿using System;
using System.Collections.Generic;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Persistence.Dtos;

namespace CD.UmbracoFormsMailchimpWorkflow.Workflow
{
    public class MailchimpWorkflow : WorkflowType
    {
        [Setting("List Id", Alias = "listId", Description = "Unique list id of the list in Mailchimp")]
        public string ListId { get; set; }

        [Setting("Field mapping", Alias = "fieldMapping", Description = "Map the Umbraco Forms fields to the fields in the Mailchimp list", View = "~/App_Plugins/CD.UmbracoFormsMailchimpWorkflow/fieldmapping.html")]
        public string FieldMapping { get; set; }

        public MailchimpWorkflow()
        {
            this.Id = new Guid("68bdc512-f67c-480f-921c-3a7a1b35c512");
            this.Name = "Push to Mailchimp";
            this.Description = "Map form fields and push data to a list in Mailchimp";
        }

        public override WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override List<Exception> ValidateSettings()
        {
            throw new NotImplementedException();
        }
    }
}