app.controller('inboxController', ['$scope', 'service', '$rootScope', '$cookieStore', '$window', '$resource', function ($scope, service, $rootScope, $cookieStore, $window, $resource) {
    $scope.compose = false;
    $scope.inbox = true;
    $scope.contacts = false;

    $scope.showCompose = function () {
        $scope.compose = true;
        $scope.inbox = false;
        $scope.contacts = false;
    };
    $scope.showInbox = function () {
        $scope.compose = false;
        $scope.inbox = true;
        $scope.contacts = false;
    };
    $scope.showContacts = function () {
        $scope.compose = false;
        $scope.inbox = false;
        $scope.contacts = true;
    };
}]);