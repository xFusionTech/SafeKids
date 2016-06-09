app.controller('parentphotoController', ['$scope', '$rootScope', '$cookieStore', 'helper', '$resource', 'Upload', '$timeout', function ($scope, $rootScope, $cookieStore, helper, $resource, Upload, $timeout) {

    var self = Object.create(null);
    var filename = '';
    self.uploading = false;
    $scope.uploadFiles = function (errFiles, file) {
        $scope.userObj = $cookieStore.get('Userdata');
        var PER_ID = ($scope.userObj !== undefined) ? $scope.userObj.result.PERS_ID : "00000000-0000-0000-0000-000000000000";
        var Token = ($scope.userObj !== undefined) ? $scope.userObj.result.TokenId : "00000000-0000-0000-0000-000000000000";

        if (errFiles.length > 0) {
            self.uploading = true;
            var data = new FormData();
            for (i = 0; i < errFiles.length; i++) {
                data.append("file" + i, errFiles[i]);
                filename = errFiles[i].name;
            }
            data.append("PersonId", PER_ID);
            data.append("Token", Token);
        }

        if (errFiles.length > 0)
            $.ajax({
                type: "POST",
                url: "http://localhost:8085/api/FileUpload?Token=" + Token + "&PER_ID=" + PER_ID,
                contentType: false,
                processData: false,
                data: data,
                success: function (messages) {                   
                    for (i = 0; i < messages.length; i++) {
                        $rootScope.$broadcast('refreshParentProfilePhoto', { name: messages[i].split("|~|")[1], Image: messages[i].split("|~|")[0] });
                    }
                    self.uploading = false;
                },
                error: function () {
                    console.log("Error while invoking the Web API");
                    self.uploading = false;
                }
            });
    }

    return self;
}]);