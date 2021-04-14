angular.module("umbraco").controller("CD.UmbracoFormsMailchimpWorkflow.FieldMapping",
    function ($scope, fieldmappingResource) {

        $scope.getMcFields = function() {
            fieldmappingResource.getMailchimpListMergeFields($scope.model.mcListId).then(function (response) {
                $scope.mcFields = response.data
            });
        }

    });