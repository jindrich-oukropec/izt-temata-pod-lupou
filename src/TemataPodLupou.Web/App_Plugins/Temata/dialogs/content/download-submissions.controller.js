angular.module("umbraco")
    .controller("Temata.DownloadSubmissionsController",
        function ($scope, navigationService) {
            var vm = this;

            vm.downloadLink = "backoffice/Temata/Submissions/Download?nodeId=" + $scope.currentNode.id;

            vm.close = function () {
                navigationService.hideDialog();
            }
        });