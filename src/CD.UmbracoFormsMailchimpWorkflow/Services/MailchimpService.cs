using CD.UmbracoFormsMailchimpWorkflow.Models.Api.Response;
using CD.UmbracoFormsMailchimpWorkflow.Models.Dto.Mailchimp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Umbraco.Core.Logging;

namespace CD.UmbracoFormsMailchimpWorkflow.Services
{
    public interface IMailchimpService
    {
        Task<IEnumerable<List>> GetMailchimpLists();
        Task<IEnumerable<MergeField>> GetMailchimpListMergeFields(string listId);
    }

    public class MailchimpService : IMailchimpService
    {
        private string MailchimpApiKey => ConfigurationManager.AppSettings.Get("Mailchimp.ApiKey");
        private string MailchimpDataCenter => MailchimpApiKey?.Substring(MailchimpApiKey.LastIndexOf("-") + 1);
        private string MailchimpBaseAddress => string.Concat("https://", MailchimpDataCenter, ".api.mailchimp.com/3.0");

        private readonly ILogger logger;

        public MailchimpService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<List>> GetMailchimpLists()
        {
            using (var client = GetMailchimpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(string.Concat(MailchimpBaseAddress, "/lists"));

                    if (JsonConvert.DeserializeObject<MailchimpListsResponse>(response) is MailchimpListsResponse mailchimpListsResponse)
                    {
                        return mailchimpListsResponse.Lists
                            .Select(l =>
                                new List
                                {
                                    Id = l.Id,
                                    Name = l.Name
                                }
                        );
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<MailchimpService>(ex);
                }

                return null;
            }
        }

        public async Task<IEnumerable<MergeField>> GetMailchimpListMergeFields(string listId)
        {
            using (var client = GetMailchimpClient())
            {
                try
                {
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
                    logger.Error<MailchimpService>(ex);
                }

                return null;
            }
        }


        private HttpClient GetMailchimpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MailchimpApiKey);

            return client;
        }
    }
}