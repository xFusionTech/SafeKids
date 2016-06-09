app.controller('createController', ['$scope', 'service', '$rootScope', '$cookieStore', '$window', '$resource', function ($scope, service, $rootScope, $cookieStore, $window, $resource) {
    $scope.submitted = false;
    $scope.messageObj = {
        Subject: '',
        Message: '',
        sendTo: 'Joshua'
    };
    $scope.userObj = $cookieStore.get('Userdata');
    $scope.save = function () {
        if ($scope.sendMessage.$valid) {
            var message = {
                Message_Type: 'Out',
                Message_To: $scope.messageObj.sendTo,
                Message_Subject: $scope.messageObj.Subject,
                Message_Body: $scope.messageObj.Message
            };
            console.log(message);
            $resource("http://localhost:8086/api/Message/:token", { token: $scope.userObj.result.TokenId }, { 'update': { method: 'PUT' } }).save(message, function (data) {
                $scope.messageObj.Subject = '';
                $scope.messageObj.Message = '';
                $scope.$parent.showInbox();
                $rootScope.updateInbox();
            });
        } else {
            $scope.submitted = true;
            $scope.validationError = true;
        }
    };
}]);