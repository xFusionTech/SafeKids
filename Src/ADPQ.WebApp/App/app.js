'use strict';
var app = angular.module('adpq', [
  'ngRoute',
  'ngResource',
  'ui.bootstrap',
  'ngCookies',
  'uiGmapgoogle-maps',
  'ngFileUpload',
  'angular-loading-bar',
  'ngAnimate',
  'angular-confirm'
]).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
}]).service('authInterceptor', function ($q, $rootScope) {
    var service = this;

    service.responseError = function (response) {
        if (response.status == 401) {
            $rootScope.logout();
        }
        return $q.reject(response);
    };
})
.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
}]);

