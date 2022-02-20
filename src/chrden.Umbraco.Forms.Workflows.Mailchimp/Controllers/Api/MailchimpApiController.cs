using chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Dto;
using chrden.Umbraco.Forms.Workflows.Mailchimp.Services;
using System.Collections.Generic;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Controllers.Api
{
    [PluginController("chrden")]
    public class MailchimpApiController : UmbracoAuthorizedJsonController
    {
        private readonly IMailchimpService _mailchimpService;

        public MailchimpApiController(IMailchimpService mailchimpService)
        {
            _mailchimpService = mailchimpService;
        }

        public IEnumerable<List> GetMailchimpLists()
            => _mailchimpService.GetMailchimpLists();

        public IEnumerable<MergeField> GetMailchimpListMergeFields(string listId)
            => _mailchimpService.GetMailchimpListMergeFields(listId);
    }
}