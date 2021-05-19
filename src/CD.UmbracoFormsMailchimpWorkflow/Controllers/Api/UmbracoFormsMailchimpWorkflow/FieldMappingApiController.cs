using CD.UmbracoFormsMailchimpWorkflow.Models.Mailchimp;
using CD.UmbracoFormsMailchimpWorkflow.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace CD.UmbracoFormsMailchimpWorkflow.Controllers.Api.UmbracoFormsMailchimpWorkflow
{
    [PluginController("CD")]
    public class FieldMappingApiController : UmbracoAuthorizedJsonController
    {
        private readonly IMailchimpService mailchimpService;

        public FieldMappingApiController(IMailchimpService mailchimpService)
        {
            this.mailchimpService = mailchimpService;
        }

        public async Task<IEnumerable<List>> GetMailchimpLists()
            => await mailchimpService.GetMailchimpLists();

        public async Task<IEnumerable<MergeField>> GetMailchimpListMergeFields(string listId)
            => await mailchimpService.GetMailchimpListMergeFields(listId);
    }
}