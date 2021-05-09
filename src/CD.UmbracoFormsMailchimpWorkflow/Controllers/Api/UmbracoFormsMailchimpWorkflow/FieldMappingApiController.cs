using CD.UmbracoFormsMailchimpWorkflow.Models.Api.Response;
using CD.UmbracoFormsMailchimpWorkflow.Models.Mailchimp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<MergeField>> GetMailchimpListMergeFields(string listId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    //client.BaseAddress = new Uri(MailchimpBaseAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MailchimpApiKey);

                    var response = await client.GetStringAsync(string.Concat(MailchimpBaseAddress, "/lists/", listId, "/merge-fields"));

                    if (JsonConvert.DeserializeObject<MailchimpMergeFieldsResponse>(response) is MailchimpMergeFieldsResponse mergeFieldsResponse)
                    {
                        var mergeFields =
                            new List<MergeField>()
                            {
                                new MergeField
                                {
                                    Name = "Email",
                                    Tag = "EMAIL"
                                }
                            };

                        mergeFields.AddRange(
                            mergeFieldsResponse.MergeFields
                            .Select(mf =>
                                new MergeField
                                {
                                    Name = mf.Name,
                                    Tag = mf.Tag,
                                }
                            )
                        );

                        return mergeFields;
                    }
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