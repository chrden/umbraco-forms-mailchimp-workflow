using CD.UmbracoFormsMailchimpWorkflow.Models.Dto.Mailchimp;
using CD.UmbracoFormsMailchimpWorkflow.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace CD.UmbracoFormsMailchimpWorkflow.Controllers.Api
{
    [PluginController("CD")]
    public class MailchimpApiController : UmbracoAuthorizedJsonController
    {
        private readonly IMailchimpService mailchimpService;

        public MailchimpApiController(IMailchimpService mailchimpService)
        {
            this.mailchimpService = mailchimpService;
        }

        public async Task<IEnumerable<List>> GetMailchimpLists()
            => await mailchimpService.GetMailchimpLists();

        public async Task<IEnumerable<MergeField>> GetMailchimpListMergeFields(string listId)
            => await mailchimpService.GetMailchimpListMergeFields(listId);
    }
}