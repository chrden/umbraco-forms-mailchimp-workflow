using System.Collections.Generic;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace CD.UmbracoFormsMailchimpWorkflow.Controllers.Api.UmbracoFormsMailchimpWorkflow
{
    [PluginController("CD")]
    public class FieldMappingApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<string> GetUmbracoFormFields() { return null; }
        public IEnumerable<string> GetMailchimpListMergeFields(int mcListId)
        {
            return null;
        }
    }
}