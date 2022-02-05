using CD.UmbracoFormsMailchimpWorkflow.Models.Api.Request;
using CD.UmbracoFormsMailchimpWorkflow.Models.Api.Response;
using CD.UmbracoFormsMailchimpWorkflow.Models.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
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
            try
            {
                using (var client = GetMailchimpClient())
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
            }
            catch (Exception ex)
            {
                logger.Error<MailchimpService>(ex);
            }

            return null;
        }

        public IEnumerable<MergeField> GetMailchimpListMergeFields(string listId)
        {
            try
            {
                using (var client = GetMailchimpClient())
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
            }
            catch (Exception ex)
            {
                logger.Error<MailchimpService>(ex);
            }

            return null;
        }

        #endregion

        #region POST

        public bool AddSubscriberToList(string listId, string emailAddress, Dictionary<string, string> mergeFields)
        {
            try
            {
                using (var client = GetMailchimpClient())
                {
                    var subscriberHash = CreateMD5(emailAddress.ToLower());
                    var uri = string.Concat(MailchimpBaseAddress, "/lists/", listId, "/members/", subscriberHash);

                    var requestObj =
                        new MailchimpAddSubscriberRequest
                        {
                            EmailAddress = emailAddress,
                            MergeFields = mergeFields
                        };

                    var response = client.PutAsJsonAsync(uri, requestObj).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var errorResponseString = response.Content.ReadAsStringAsync();
                        var error = JsonConvert.DeserializeObject<MailchimpErrorResponse>(errorResponseString.Result);

                        logger.Error<MailchimpService>("Umbraco Forms Mailchimp Workflow error: {Title} ({Status}) - {ErrorMessage} ({InstanceId}). Mailchimp list id: {ListId}", error.Title, error.Status, error.Detail, error.Instance, listId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error<MailchimpService>(ex);
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

        /// <summary>
        /// https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string?rq=1
        /// </summary>
        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}