angular.module("umbraco").controller("CD.UmbracoFormsMailchimpWorkflow.FieldMapping",
    function ($scope, fieldmappingResource) {

        $scope.model.showListsDropdown = false;
        $scope.model.showFieldsDropdown = false;

        fieldmappingResource.getMailchimpLists().then(function (response) {
            $scope.model.showListsDropdown = true;
            $scope.model.mcLists = response
        });

        $scope.getListFields = function () {
            fieldmappingResource.getMailchimpListMergeFields($scope.model.mcListId).then(function (response) {
                $scope.model.showFieldsDropdown = true;
                $scope.model.mcFields = response
            });
        }

    });