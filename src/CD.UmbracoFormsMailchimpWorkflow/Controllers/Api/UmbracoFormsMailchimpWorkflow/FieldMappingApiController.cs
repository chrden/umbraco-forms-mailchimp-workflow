using CD.UmbracoFormsMailchimpWorkflow.Models.Response.Lists;
using CD.UmbracoFormsMailchimpWorkflow.Models.Response.MergeFields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace CD.UmbracoFormsMailchimpWorkflow.Controllers.Api.UmbracoFormsMailchimpWorkflow
{
    [PluginController("CD")]
    public class FieldMappingApiController : UmbracoAuthorizedJsonController
    {
        private string MailchimpApiKey => ConfigurationManager.AppSettings.Get("Mailchimp.ApiKey");
        private string MailchimpDataCenter => MailchimpApiKey?.Substring(MailchimpApiKey.LastIndexOf("-") + 1);
        private string MailchimpBaseAddress => string.Concat("https://", MailchimpDataCenter, ".api.mailchimp.com/3.0");

        private readonly ILogger logger;

        public FieldMappingApiController(ILogger logger)
        {
            this.logger = logger;
        }

        public IEnumerable<string> GetUmbracoFormFields() { return null; }

        public IEnumerable<Merge_Field> GetMailchimpListMergeFields(string listId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    //client.BaseAddress = new Uri(MailchimpBaseAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MailchimpApiKey);

                    var response = client.GetStringAsync(string.Concat(MailchimpBaseAddress, "/lists/", listId, "/merge-fields")).Result;

                    return JsonConvert.DeserializeObject<MailchimpMergeFieldsResponse>(response).merge_fields;
                }
                catch (Exception ex)
                {
                    logger.Error<FieldMappingApiController>(ex);
                }

                return null;
            }
        }

        public IEnumerable<MailchimpList> GetMailchimpLists()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    //client.BaseAddress = new Uri(MailchimpBaseAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MailchimpApiKey);

                    var response = client.GetStringAsync(string.Concat(MailchimpBaseAddress, "/lists")).Result;

                    return JsonConvert.DeserializeObject<MailchimpListsResponse>(response).lists;
                }
                catch (Exception ex)
                {
                    logger.Error<FieldMappingApiController>(ex);
                }

                return null;
            }
        }
    }
}