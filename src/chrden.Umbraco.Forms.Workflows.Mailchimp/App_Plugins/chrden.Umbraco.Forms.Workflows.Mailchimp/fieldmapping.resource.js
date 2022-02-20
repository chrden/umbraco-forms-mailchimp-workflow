angular.module("umbraco.resources").factory("fieldmappingResource",
    function ($q, $http, umbRequestHelper) {
        // the factory object returned
        return {
            getMailchimpListMergeFields: function (listId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/chrden/MailchimpApi/GetMailchimpListMergeFields", { params: { listId: listId } }), "Failed to retrieve Mailchimp list fields");
            },
            getMailchimpLists: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/chrden/MailchimpApi/GetMailchimpLists"), "Failed to retrieve Mailchimp lists");
            }
        }
    });