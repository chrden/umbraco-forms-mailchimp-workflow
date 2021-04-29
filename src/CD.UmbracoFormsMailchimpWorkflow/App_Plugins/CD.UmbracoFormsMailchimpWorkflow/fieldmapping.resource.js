angular.module("umbraco.resources").factory("fieldmappingResource",
    function ($q, $http, umbRequestHelper) {
        // the factory object returned
        return {
            getUmbracoFormFields: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/CD/FieldMappingApi/GetUmbracoFormFields"), "Failed to retrieve Umbraco Forms data");
            },
            getMailchimpListMergeFields: function (listId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/CD/FieldMappingApi/GetMailchimpListMergeFields", { params: { listId: listId } }), "Failed to retrieve Mailchimp list fields");
            },
            getMailchimpLists: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/CD/FieldMappingApi/GetMailchimpLists"), "Failed to retrieve Mailchimp lists");
            }
        }
    });