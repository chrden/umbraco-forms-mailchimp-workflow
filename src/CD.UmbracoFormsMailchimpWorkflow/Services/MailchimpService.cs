using CD.UmbracoFormsMailchimpWorkflow.Models.Api.Request;
using CD.UmbracoFormsMailchimpWorkflow.Models.Api.Response;
using CD.UmbracoFormsMailchimpWorkflow.Models.Dto.Mailchimp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Umbraco.Core.Logging;

namespace CD.UmbracoFormsMailchimpWorkflow.Services
{
    public interface IMailchimpService
    {
        IEnumerable<List> GetMailchimpLists();
        IEnumerable<MergeField> GetMailchimpListMergeFields(string listId);
        bool AddSubscriberToList(string listId, string emailAddress, Dictionary<string, string> mergeFields);
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

        #region GET

        public IEnumerable<List> GetMailchimpLists()
        {
            using (var client = GetMailchimpClient())
            {
                try
                {
                    var response = client.GetStringAsync(string.Concat(MailchimpBaseAddress, "/lists")).Result;

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

        public IEnumerable<MergeField> GetMailchimpListMergeFields(string listId)
        {
            using (var client = GetMailchimpClient())
            {
                try
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

                    var response = client.GetStringAsync(string.Concat(MailchimpBaseAddress, "/lists/", listId, "/merge-fields")).Result;

                    var mergeFieldsResponse = JsonConvert.DeserializeObject<MailchimpMergeFieldsResponse>(response);

                    if (mergeFieldsResponse != null && mergeFieldsResponse.MergeFields.Any())
                    {
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
                    }

                    return mergeFields;
                }
                catch (Exception ex)
                {
                    logger.Error<MailchimpService>(ex);
                }

                return null;
            }
        }

        #endregion

        #region POST

        public bool AddSubscriberToList(string listId, string emailAddress, Dictionary<string, string> mergeFields)
        {
            using (var client = GetMailchimpClient())
            {
                var uri = string.Concat(MailchimpBaseAddress, "/lists/", listId, "/members");

                var requestObj =
                    new MailchimpAddSubscriberRequest
                    {
                        EmailAddress = emailAddress,
                        MergeFields = mergeFields
                    };

                var response = client.PostAsJsonAsync(uri, requestObj).Result;

                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    logger.Error<MailchimpService>("Add subscriber to list with id {listId} failed.", listId);
                }
             }

            return false;
        }

        #endregion

        private HttpClient GetMailchimpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MailchimpApiKey);

            return client;
        }
    }
}