app.controller('mainController', ['$scope', 'service', '$rootScope', 'helper', '$cookieStore', '$window', '$templateCache', function ($scope, service, $rootScope, helper, $cookieStore, $window, $templateCache) {

    var self = Object.create(null);

	$scope.sessionObject = $cookieStore.get('session');
	$rootScope.tabset = [
        { heading: 'Parent Profile', page: '/App/pages/main/parent/index.html', disabled: false, icon: 'glyphicon-user' },
        { heading: 'Child(ren) Profile', page: '/App/pages/main/child/index.html', disabled: false, icon: 'glyphicon-heart' },
        { heading: 'Facilities Locator', page: '/App/pages/main/facilitiesLocator/index.html', disabled: false, icon: 'glyphicon-map-marker' },
        { heading: 'Message Inbox', page: '/App/pages/main/inbox/index.html', disabled: false, icon: 'glyphicon-envelope' },
    ];
	
    $rootScope.currentPage = '/App/pages/main/parent/index.html';
    $rootScope.pageIndex = 0;

    self.loadTemplate = function (page, index) {
        if(index === 2)
         $templateCache.remove(page);
        $rootScope.currentPage = page;
        $rootScope.pageIndex = index;
    }
    
    $rootScope.loggedin = true;
    $rootScope.loginPage = false;
    $rootScope.profileName = '';

    $rootScope.logout = function () {
        $cookieStore.remove("session");
        $cookieStore.remove("Userdata");
        $cookieStore.remove("registrationObj");
        $cookieStore.remove("Verification");
        $window.location.href = "/App/index.html#/";
    }

    if (!helper.isloggedin($scope.sessionObject))
        $window.location.href = "/App/index.html#/";

    self.isRegistering = helper.isRegistering($cookieStore.get('session'));
    if (!self.isRegistering) {
        $scope.userObj = $cookieStore.get('Userdata');
        $rootScope.profileName = $scope.userObj.result.personname.PERS_FNAME + " " + $scope.userObj.result.personname.PERS_LNAME;
    } else {
        self.registrationObj = $cookieStore.get('registrationObj');
        $rootScope.profileName = self.registrationObj.firstname + " " + self.registrationObj.lastname;
    }
    

    return self;

}]).directive('numberOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope) {
            scope.$watch('wks.number', function (newValue, oldValue) {
                var arr = String(newValue).split("");
                if (arr.length === 0) return;
                if (arr.length === 1 && (arr[0] == '-' || arr[0] === '.')) return;
                if (arr.length === 2 && newValue === '-.') return;
                if (isNaN(newValue)) {
                    scope.wks.number = oldValue;
                }
            });
        }
    };
});