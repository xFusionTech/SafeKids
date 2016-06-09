app.controller('disabilityController', ['$scope', '$rootScope', '$cookieStore', 'helper', '$resource', 'Upload', '$timeout', function ($scope, $rootScope, $cookieStore, helper, $resource, Upload, $timeout) {
    var self = Object.create(null);
    var filename = '';
    self.uploading = false;

    $scope.uploadFiles = function (errFiles, file) {
        $scope.userObj = $cookieStore.get('Userdata');
        var PER_ID = $scope.userObj.result.PERS_ID;
        var Token = $scope.userObj.result.TokenId
        
        if (errFiles !== null) {
            self.uploading = true;
            if (errFiles.type !== 'application/pdf') {
                $rootScope.$broadcast('refreshInvalidDisabilityPdf', { name: true });
                return false;
            }

            if (errFiles.size > 0) {
                var data = new FormData();
                data.append("file", errFiles);
                filename = errFiles.name;
            }

            $scope.f = errFiles;
            $scope.errFile = errFiles;
            if (errFiles.size > 0)
                $.ajax({
                    type: "POST",
                    url: "http://localhost:8085/api/FileUpload?Token=" + Token + "&PER_ID=" + PER_ID,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (messages) {
                        for (i = 0; i < messages.length; i++) {
                            $rootScope.$broadcast('refreshDisability', { name: messages[i], filename: filename });
                        }
                        self.uploading = false;
                    },
                    error: function () {
                        console.log("Error while invoking the Web API");
                        self.uploading = false;
                    }
                });
        }
        else
        {
            if (file.size > 0 || file.$error)
            {
                $rootScope.$broadcast('refreshInvalidDisabilityPdf', { name: true });
                self.uploading = false;
                return false;
            }
        }

    }
    return self;
}]);

