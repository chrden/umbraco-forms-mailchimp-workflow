using chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Api.Request;
using chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Api.Response;
using chrden.Umbraco.Forms.Workflows.Mailchimp.Models.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Services
{
    public interface IMailchimpService
    {
        IEnumerable<List> GetMailchimpLists();
        IEnumerable<MergeField> GetMailchimpListMergeFields(string listId);
        bool AddSubscriberToList(string listId, string emailAddress, Dictionary<string, string> mergeFields);
    }

    public class MailchimpService : IMailchimpService
    {
        private string MailchimpApiKey;
        private string MailchimpDataCenter;
        private string MailchimpBaseAddress;

        private readonly ILogger<MailchimpService> _logger;

        public MailchimpService(ILogger<MailchimpService> logger, IConfiguration configuration)
        {
            _logger = logger;

            MailchimpApiKey = configuration.GetValue<string>("Mailchimp:ApiKey");
            MailchimpDataCenter = MailchimpApiKey?.Substring(MailchimpApiKey.LastIndexOf("-") + 1);
            MailchimpBaseAddress = string.Concat("https://", MailchimpDataCenter, ".api.mailchimp.com/3.0");
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
                _logger.LogError(ex, "Error retreiving Mailchimp lists");
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
                _logger.LogError(ex, "Error retreiving merge fields for List Id: {listId}", listId);
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
                            Members =
                                new List<MailchimpAddSubscriberRequest.MemberDetails>
                                {
                                    new MailchimpAddSubscriberRequest.MemberDetails 
                                    {
                                        EmailAddress = emailAddress,
                                        MergeFields = mergeFields
                                    }
                                }
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

                        _logger.LogError("Umbraco Forms Mailchimp Workflow error: {Title} ({Status}) - {ErrorMessage} ({InstanceId}). Mailchimp list id: {ListId}", error.Title, error.Status, error.Detail, error.Instance, listId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding subscriber to list");
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