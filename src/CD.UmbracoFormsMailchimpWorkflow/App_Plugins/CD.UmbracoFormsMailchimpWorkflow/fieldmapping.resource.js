angular.module("umbraco.resources").factory("fieldmappingResource",
    function ($q, $http, umbRequestHelper) {
        // the factory object returned
        return {
            getUmbracoFormFields: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/CD/FieldMappingApi/GetUmbracoFormFields"), "Failed to retrieve Umbraco Forms data");
            },
            getMailchimpListMergeFields: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/CD/FieldMappingApi/GetMailchimpListMergeFields"), "Failed to retrieve Mailchimp data");
            }
        }
    });