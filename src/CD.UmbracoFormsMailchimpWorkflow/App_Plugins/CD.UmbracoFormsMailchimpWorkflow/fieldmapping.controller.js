angular.module("umbraco").controller("CD.UmbracoFormsMailchimpWorkflow.FieldMapping",
    function ($scope, fieldmappingResource) {

        $scope.model.error = false;
        $scope.model.fieldMappings = [];
        $scope.model.showListsDropdown = false;

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
                    $scope.model.showListsDropdown = true;
                    $scope.model.lists = response;
                }
                else {
                    $scope.model.error = true;
                    $scope.model.showListsDropdown = false;
                }
            }, function () {
                $scope.model.error = true;
                $scope.model.showListsDropdown = false;
            });

            if (!$scope.model.error && $scope.setting.value) {
                var result = JSON.parse($scope.setting.value);

                $scope.model.showListsDropdown = true;
                $scope.model.listId = result.listId;
                $scope.getListFields(result.fieldMappings);

                $.each(result.fieldMappings, function (i, item) {
                    $scope.model.fieldMappings.push({
                        formFieldAlias: item.formFieldAlias,
                        listFieldId: item.listFieldId,
                        staticValue: item.staticValue,
                        isStatic: item.isStatic
                    });
                });
            }
        }

        $scope.getListFields = function (mappings) {
            if ($scope.model.listId) {
                fieldmappingResource.getMailchimpListMergeFields($scope.model.listId).then(function (response) {
                    $scope.model.listFields = response;
                    if (mappings) {
                        $scope.model.fieldMappings = mappings;
                    }
                    else {
                        $scope.model.fieldMappings = [];
                    }
                });
            }
            else {
                $scope.model.fieldMappings = [];
            }

            save();
        }

        $scope.addFieldMapping = function (isStatic) {
            save();

            $scope.model.fieldMappings.push({
                formFieldAlias: "",
                listFieldId: "",
                staticValue: "",
                isStatic
            });
        };

        $scope.deleteFieldMapping = function (i) {
            $scope.model.fieldMappings.splice(i, 1);

            save();
        };

        function save() {
            $scope.setting.value =
                JSON.stringify({
                    listId: $scope.model.listId,
                    fieldMappings: $scope.model.fieldMappings
                });
        }

        init();
    }
);