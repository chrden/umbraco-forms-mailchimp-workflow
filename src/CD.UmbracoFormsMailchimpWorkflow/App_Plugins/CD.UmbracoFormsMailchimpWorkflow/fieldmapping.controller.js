angular.module("umbraco").controller("CD.UmbracoFormsMailchimpWorkflow.FieldMapping",
    function ($scope, fieldmappingResource) {

        $scope.model.dropdownOpen = false;
        $scope.model.items = [
            { "name": "Item 1" },
            { "name": "Item 2" },
            { "name": "Item 3" }
        ];

        $scope.model.toggle = toggle;
        $scope.model.close = close;
        $scope.model.select = select;

        function toggle() {
            $scope.model.dropdownOpen = true;
        }

        function close() {
            $scope.model.dropdownOpen = false;
        }

        function select(item) {
            console.log(item);
        }

        $scope.getMcFields = function () {
            $scope.model.dropdownOpen = true;

            //fieldmappingResource.getMailchimpListMergeFields($scope.model.mcListId).then(function (response) {
            //    $scope.mcFields = response.data
            //});
        }

    });