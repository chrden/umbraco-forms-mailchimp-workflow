using System.Collections;
using System.Collections.Generic;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace CD.UmbracoFormsMailchimpWorkflow.Controllers.Api.UmbracoFormsMailchimpWorkflow
{
    [PluginController("CD")]
    public class FieldMappingApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<string> GetUmbracoFormFields() { }
        public IEnumerable<string> GetMailchimpListMergeFields() { }
    }
}