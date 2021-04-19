using System;
using System.Collections.Generic;

namespace CD.UmbracoFormsMailchimpWorkflow.Models.Response.Lists
{
    public class MailchimpListsResponse
    {
        public IEnumerable<MailchimpList> lists { get; set; }
        public int total_items { get; set; }
        public Constraints constraints { get; set; }
        public IEnumerable<_Links> _links { get; set; }
    }

    public class Constraints
    {
        public bool may_create { get; set; }
        public int max_instances { get; set; }
        public int current_total_instances { get; set; }
    }

    public class MailchimpList
    {
        public string id { get; set; }
        public int web_id { get; set; }
        public string name { get; set; }
        public Contact contact { get; set; }
        public string permission_reminder { get; set; }
        public bool use_archive_bar { get; set; }
        public Campaign_Defaults campaign_defaults { get; set; }
        public string notify_on_subscribe { get; set; }
        public string notify_on_unsubscribe { get; set; }
        public DateTime date_created { get; set; }
        public int list_rating { get; set; }
        public bool email_type_option { get; set; }
        public string subscribe_url_short { get; set; }
        public string subscribe_url_long { get; set; }
        public string beamer_address { get; set; }
        public string visibility { get; set; }
        public bool double_optin { get; set; }
        public bool has_welcome { get; set; }
        public bool marketing_permissions { get; set; }
        public object[] modules { get; set; }
        public Stats stats { get; set; }
        public IEnumerable<_Links> _links { get; set; }
    }

    public class Contact
    {
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
    }

    public class Campaign_Defaults
    {
        public string from_name { get; set; }
        public string from_email { get; set; }
        public string subject { get; set; }
        public string language { get; set; }
    }

    public class Stats
    {
        public int member_count { get; set; }
        public int unsubscribe_count { get; set; }
        public int cleaned_count { get; set; }
        public int member_count_since_send { get; set; }
        public int unsubscribe_count_since_send { get; set; }
        public int cleaned_count_since_send { get; set; }
        public int campaign_count { get; set; }
        public DateTime campaign_last_sent { get; set; }
        public int merge_field_count { get; set; }
        public int avg_sub_rate { get; set; }
        public int avg_unsub_rate { get; set; }
        public int target_sub_rate { get; set; }
        public float open_rate { get; set; }
        public float click_rate { get; set; }
        public DateTime last_sub_date { get; set; }
        public DateTime last_unsub_date { get; set; }
    }

    public class _Links
    {
        public string rel { get; set; }
        public string href { get; set; }
        public string method { get; set; }
        public string targetSchema { get; set; }
        public string schema { get; set; }
    }
}