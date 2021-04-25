angular.module("umbraco").controller("CD.UmbracoFormsMailchimpWorkflow.FieldMapping",
    function ($scope, fieldmappingResource) {

        $scope.model.fieldMappings = [];
        $scope.model.showListsDropdown = false;
        $scope.model.showFieldsDropdown = false;

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

        fieldmappingResource.getMailchimpLists().then(function (response) {
            $scope.model.showListsDropdown = true;
            $scope.model.lists = response
        });

        $scope.getListFields = function () {
            if ($scope.model.listId) {
                fieldmappingResource.getMailchimpListMergeFields($scope.model.listId).then(function (response) {
                    $scope.model.showFieldsDropdown = true;
                    $scope.model.listFields = response;
                    $scope.model.fieldMappings = [];
                    //$scope.addFieldMapping();
                });
            }
            else {
                $scope.model.fieldMappings = [];
            }
        }

        $scope.addFieldMapping = function (isStatic) {
            $scope.model.fieldMappings.push({
                formFieldAlias: "",
                listFieldId: "",
                staticValue: "",
                isStatic: isStatic
            });

        };

        $scope.deleteFieldMapping = function (i) {
            $scope.model.fieldMappings.splice(i, 1);
        };
    });