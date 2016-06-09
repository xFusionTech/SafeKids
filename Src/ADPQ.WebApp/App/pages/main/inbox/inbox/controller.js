app.controller('inboxMessageController', ['$scope', 'service', '$rootScope', '$cookieStore', '$parse', '$window', '$resource', 'helper', function ($scope, service, $rootScope, $cookieStore, $parse, $window, $resource, helper) {
    $scope.isRegistering = helper.isRegistering($cookieStore.get('session'));
    if (!$scope.isRegistering)
    {
        $scope.userObj = $cookieStore.get('Userdata');
        $scope.messages = [];
        $scope.textarea = [];
        $scope.fieldValid = [];
        $rootScope.unreadCount = 0;

        $rootScope.updateInbox = function () {
            $scope.messages = [];
            $resource("http://localhost:8086/api/Message/:token", { token: $scope.userObj.result.TokenId }).get(function (data) {
                $scope.messages = data.result;
				console.log($scope.messages);
                $rootScope.unreadCount = data.totalUnread;
            });
        };

        $scope.markRead = function (message) {
            //$scope.messages = [];
            console.log(message);
			message.Message_IsRead=true;
            console.log(message);
            //$resource("http://localhost:8086/api/Message/:token/:id", { token: $scope.userObj.result.TokenId, id: message.Message_ID }).save(function (data) {
                //$scope.messages = data.result;
                //$rootScope.unreadCount = data.totalUnread;
            //});
        };

        $scope.remove = function (message) {
            $scope.messages = [];
            console.log(message);
            $resource("http://localhost:8086/api/Message/:token/:id", { token: $scope.userObj.result.TokenId, id: message.Message_Header }).get(function (data) {
                $scope.messages = data.result;
                $rootScope.unreadCount = data.totalUnread;
            });
        };

        $scope.reply = function (input, index) {
            var model = $parse("namesForm_"+index+"_submited");
            model.assign($scope, true);

            var message = {
                Message_Type: 'Out',
                Message_To: input.Message_To,
                Message_Subject: input.Message_Subject,
                Message_Body: $scope.textarea[index],
                Person_ID: input.Person_ID,
                Message_Header: input.Message_Header,
            };
            $resource("http://localhost:8086/api/Message/:token", { token: $scope.userObj.result.TokenId }, { 'update': { method: 'PUT' } }).save(message, function (data) {
                //console.log(data);
                $rootScope.updateInbox();
            });
        };
    }
}]);