using CD.UmbracoFormsMailchimpWorkflow.Models.Dto;
using CD.UmbracoFormsMailchimpWorkflow.Services;
using System.Collections.Generic;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace CD.UmbracoFormsMailchimpWorkflow.Controllers.Api
{
    [PluginController("CD")]
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