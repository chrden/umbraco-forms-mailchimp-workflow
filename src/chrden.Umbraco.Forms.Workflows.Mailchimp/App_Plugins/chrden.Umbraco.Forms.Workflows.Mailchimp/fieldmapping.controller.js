angular.module("umbraco").controller("chrden.Umbraco.Forms.Workflows.Mailchimp.FieldMapping",
    function ($scope, fieldmappingResource) {

        $scope.model.error = false;
        $scope.model.loading = true;
        $scope.model.fieldMappings = [];

        $scope.model.mapBtn = {
            default: {
                labelKey: "mapButton_defaultLabel",
                handler: function () {
                    $scope.addFieldMapping(false);
                }
            },
            secondary: [
                {
                    labelKey: "mapButton_secondaryLabel",
                    handler: function () {
                        $scope.addFieldMapping(true);
                    }
                }
            ]
        }

        function init() {
            fieldmappingResource.getMailchimpLists().then(function (response) {
                if (response) {
                    $scope.model.lists = response;
                }
                else {
                    $scope.model.error = true;
                }
                $scope.model.loading = false;
            }, function () {
                $scope.model.error = true;
                $scope.model.loading = false;
            });

            if (!$scope.model.error && $scope.setting.value) {
                var result = JSON.parse($scope.setting.value);

                $scope.model.showListsDropdown = true;
                $scope.model.listId = result.listId;
                $scope.getListFields();

                if (result.fieldMappings) {
                    $scope.model.fieldMappings = result.fieldMappings;
                }
            }
        }

        $scope.getListFields = function () {
            if ($scope.model.listId) {
                fieldmappingResource.getMailchimpListMergeFields($scope.model.listId).then(function (response) {
                    $scope.model.listFields = response;
                });
            }
            else {
                $scope.model.fieldMappings = [];
            }
        }

        $scope.addFieldMapping = function (isStatic) {
            save();

            $scope.model.fieldMappings.push({
                formFieldAlias: "",
                listFieldTag: "",
                staticValue: "",
                isStatic
            });
        };

        $scope.deleteFieldMapping = function (i) {
            $scope.model.fieldMappings.splice(i, 1);

            save();
        };

        $scope.saveMapping = function () {
            save();
        };

        function save() {
            if ($scope.model.listId && $scope.model.fieldMappings.length > 0) {
                $scope.setting.value =
                    JSON.stringify({
                        listId: $scope.model.listId,
                        fieldMappings: $scope.model.fieldMappings
                    });
            }
            else {
                $scope.setting.value = null;
            }
        }

        init();
    }
);